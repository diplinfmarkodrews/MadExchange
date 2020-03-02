using MadXchange.Connector.Domain.Models;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Exceptions;
using MadXchange.Exchange.Handler;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Types;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MadXchange.Connector.Services
{

    public enum ConnectionStatus 
    {
        Unspecified = 0,
        Offline = 1,
        Connected = 2,
        Subscribed = 3,
        DisConnected = 4,
        Error = 5
    }

    
    public sealed class WebSocketConnection : IDisposable
    {
       
        public Guid Id { get; }
        public Xchange Xchange { get; }
        public bool IsPublic { get; } 
        public Dictionary<string, SocketSubscription> Subscriptions { get; } = new Dictionary<string, SocketSubscription>();
        private ClientWebSocket _clientWebSocket;       
        public TimeSpan KeepAliveInterval { get;  }
        public string SocketUrl { get; }
        public ConnectionStatus Status { get; internal set; } = ConnectionStatus.Offline;
        private readonly IConnectionDataHandler _clientSocketMessageHandle;
        private readonly ISignRequestsService _signRequestService;
        private bool _isAuthUrl;
        private Dictionary<string, RequestInvocation> _requestInvocations = new Dictionary<string, RequestInvocation>();

        public WebSocketConnection(IConnectionDataHandler socketMessageHandler, 
                                        XchangeDescriptor exchangeDescriptor, 
                          IEnumerable<SocketSubscription> subscriptions, 
                                     ISignRequestsService signService, 
                                                     Guid connectionId = default, 
                                                     bool ispublic = true)
        {

            _clientSocketMessageHandle = socketMessageHandler;         
            Id = connectionId == default ? Guid.NewGuid() : connectionId;
            IsPublic = ispublic;
            Xchange = exchangeDescriptor.SocketDescriptor.Xchange;
            SocketUrl = exchangeDescriptor.SocketDescriptor.SocketUrl;           
            KeepAliveInterval = TimeSpan.FromSeconds(exchangeDescriptor.SocketDescriptor.KeepAliveInterval);
            _isAuthUrl = !string.IsNullOrEmpty(exchangeDescriptor.SocketDescriptor.AuthUrl);
            _signRequestService = signService;
            foreach (var s in subscriptions)
            {
                s.SetupReturnType(exchangeDescriptor.DomainTypes[$"{s.Channel}Dto"]);
                Subscriptions.Add(s.Topic, s);
            }
        }


        public async Task StartConnectionAsync()
        {
            // also check if connection was lost, that's probably why we get called multiple times.
            if (_clientWebSocket == null || _clientWebSocket.State != WebSocketState.Open)
            {
                // create a new web-socket so the next connect call works.
                _clientWebSocket?.Dispose();
                _clientWebSocket = CreateClientWebSocket();

            }
            // don't do anything, we are already connected.
            else
                return;
            var url = SocketUrl;
            if (!IsPublic && _isAuthUrl)
                url = _signRequestService.SignSocketUrl(Id, SocketUrl);
            bool initialized = false;

            await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None).ConfigureAwait(false);
            Task.Run(() => StartListenTcp().ConfigureAwait(false));
            var request = InitOnConnected();
            if (!initialized && request != default)
                await SendRequestAsync(request, CancellationToken.None);

        }

        private ClientWebSocket CreateClientWebSocket()
        {
            var client = new ClientWebSocket();            
            client.Options.KeepAliveInterval = KeepAliveInterval;            
            return client;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private void SetStatus(ConnectionStatus p) => Status = p;

        private async Task OnDisconnected()
        {
            ResetSubscriptions();
            SetStatus(ConnectionStatus.DisConnected);
            await StartConnectionAsync();
        }

        /// <summary>
        /// listens for incoming data while websocket state is open
        /// data is evaluated in ctrl / data messages and handled appropriately
        /// </summary>
        /// <returns></returns>
        private async Task StartListenTcp()
        {
            await Receive(async (result, serializedMessage) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = serializedMessage.FromJson<SocketMessageDto>();
                    if (message.Data != null)
                        await HandleDataMessage(message).ConfigureAwait(false);

                    else
                        await HandleCtrlMessage(message).ConfigureAwait(false);
                    return;
                }
                else if (result.MessageType == WebSocketMessageType.Close && _clientWebSocket.CloseStatus != WebSocketCloseStatus.NormalClosure)
                {
                    try
                    {
                        await OnDisconnected();
                    }
                    catch (WebSocketException)
                    {
                        throw; //let's not swallow any exception for now
                    }

                    return;
                }
            }).ConfigureAwait(false);
        }


                           
        /// <summary>
        /// Receive function on the lowest level.
        /// 
        /// </summary>
        /// <param name="handleMessage"></param>
        /// <returns></returns>
        private async Task Receive(Action<WebSocketReceiveResult, string> handleMessage)
        {
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                string message = null;
                WebSocketReceiveResult result = null;
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            message = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                    handleMessage(result, message);
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        _clientWebSocket.Abort();
                    }
                }
            }

            await OnDisconnected();
        }

        /// <summary>
        /// Method to Send a SocketRequest, is only send if socket is open
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private async Task SendRequestAsync(SocketRequest message, CancellationToken token)
        {
            if (_clientWebSocket?.State != WebSocketState.Open)
                return;
         
            var encodedMessage = Encoding.UTF8.GetBytes(message.Parameter.ToJson());
            await _clientWebSocket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage,
                                                                 offset: 0,
                                                                  count: encodedMessage.Length),
                              messageType: WebSocketMessageType.Text,
                             endOfMessage: true,
                        cancellationToken: token).ConfigureAwait(false);
        }
        /// <summary>
        /// method to 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest OnCtrlMessage(SocketMessageDto message)
        {
           
            RequestInvocation request = default;
           
            var invocationFound = _requestInvocations.Remove(message.Topic, out request);
            if (invocationFound)
            {
                bool requestSuccess = message.Success.GetValueOrDefault(false);
                request.RequestCompletionSource.TrySetResult(new SocketInvocationResult() { SocketMethod = request.Request.Method, 
                                                                                              RequestKey = request.RequestKey, 
                                                                                                  Result = requestSuccess });                
                if (request.Request.Method == SocketMethod.Subscribe)
                {
                    var subscription = Subscriptions.GetValueOrDefault(message.Topic);
                    if (subscription != default)
                        subscription.IsSubscribed = requestSuccess;
                }
                if (requestSuccess)
                {
                    var newRequest = CreateSubscriptionRequestFromInactive();
                    if (newRequest == default) SetStatus(ConnectionStatus.Subscribed);
                    return newRequest;
                }
                throw new SocketRequestException(Id, $"SocketRequest failed, please file the response:\n{message.Dump()}");

            }
            throw new InvalidOperationException($"InvocationDescriptor was not found\n{message.Dump()}");            
        }


        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest InitOnConnected() 
        {
            Status = ConnectionStatus.Connected;
            ResetSubscriptions();
            if (!IsPublic && !_isAuthUrl)
                return CreateAuthRequest();            
            return CreateSubscriptionRequestFromInactive();
        }

        /// <summary>
        /// Connection generates SocketRequest object from predefined objectDictionaries. 
        /// can be optimized later on. but now it gives us more flexibility.
        /// 
        /// - CreateSubscriptionFromInactive: incremently goes through all subscriptions of a connection and activates it.
        /// - foreach request type there is afunction to create it.
        /// - all functions are mapped to create new request, to setup invocationHandle, a callback to register requests and a timeout
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateSubscriptionRequestFromInactive()
            => CreateSubscriptionRequest(GetInActiveSubscription());

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateSubscriptionRequest(SocketSubscription subscription)
            => CreateNewRequest(SocketMethod.Subscribe, new string[] { subscription.Topic });

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateUnsubscribeRequest(SocketSubscription subscription)
            => CreateNewRequest(SocketMethod.Unsubscribe, new string[] { subscription.Topic });

        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateAuthRequest()
        {
            string[] authRequest =  _signRequestService.CreatSocketAuthString(Id);
            return CreateNewRequest(SocketMethod.Auth, authRequest);
        }
              

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateNewRequest(SocketMethod method, string[] requestString) 
        {
            var request = new SocketRequest(method, requestString);            
            var topic = $"{request.Method.ToString()}{requestString}";
            TaskCompletionSource<SocketInvocationResult> task = new TaskCompletionSource<SocketInvocationResult>();
            new CancellationTokenSource(1000 * 60).Token.Register(() =>
                    {
                        _requestInvocations.Remove(topic); 
                        task.SetException(new TimeoutException($"Timeout, {Id}: did not receive a response to request {topic}")); 
                    });
            _requestInvocations[topic] = new RequestInvocation(topic, request, task);            
            return request;
        }
      

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal void ResetSubscriptions()
            => Subscriptions.Values.Each(s => s.IsSubscribed = false);
        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal SocketSubscription GetInActiveSubscription()
            => Subscriptions.Values.FirstOrDefault(s => s.IsSubscribed == false);
        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal void SetSubscriptionActive(Guid subscriptionId)
            => Subscriptions.Values.FirstOrDefault(s=>s.Id == subscriptionId);
        
        private Task HandleDataMessage(SocketMessageDto socketMessage) 
        {
            SocketSubscription subscription = GetSubscription(socketMessage.Topic);  
            
            object result = TypeSerializer.DeserializeFromString(socketMessage.Data, subscription.ReturnType);
            _clientSocketMessageHandle.HandleDataAsync(new SocketMsgPack(Id, Xchange, result, socketMessage.Timestamp));
            return Task.CompletedTask;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketSubscription GetSubscription(string topic)
            => Subscriptions[topic];
       
        private Task HandleCtrlMessage(SocketMessageDto message)
        {          
            var socketRequest = OnCtrlMessage(message);
            if (socketRequest != default)
                return Task.FromResult(SendRequestAsync(socketRequest, CancellationToken.None).ConfigureAwait(false));

            return Task.CompletedTask;
        }


        private async Task ReconnectAsync() 
        {
            Status = ConnectionStatus.DisConnected;
            _clientWebSocket.Abort();           
            await StartConnectionAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// todo: write proper disposal routine
        /// </summary>
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _clientWebSocket?.Abort();
                    _clientWebSocket = null;
                    _clientWebSocket.Dispose();
                                      
                }
                Subscriptions.Clear();
                _requestInvocations.Clear();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WebSocketConnection()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}