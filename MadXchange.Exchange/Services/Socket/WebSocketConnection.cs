using MadXchange.Connector.Domain.Models;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Handler;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MadXchange.Exchange.Services.Socket
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
        public ConnectionStatus Status { get; private set; } = ConnectionStatus.Offline;
        private readonly IConnectionDataHandler _clientSocketMessageHandle;
        private readonly ISignRequestsService _signRequestService;
        private bool _isAuthUrl;
        private readonly ILogger _logger;
        private readonly OpenTracing.ITracer _tracer;
        private ConcurrentBag<RequestInvocation> _requestInvocations = new ConcurrentBag<RequestInvocation>();

        public WebSocketConnection(IConnectionDataHandler socketMessageHandler, 
                                  Types.XchangeDescriptor exchangeDescriptor,                            
                                     ISignRequestsService signService,
                                      OpenTracing.ITracer tracer,
                             ILogger<WebSocketConnection> logger,
                                       (string, string)[] subscriptionTags = null,
                                                     Guid connectionId = default,                                                   
                                                     bool ispublic = true)

        {

            _logger = logger;
            _tracer = tracer;
            _clientSocketMessageHandle = socketMessageHandler;         
            Id = connectionId == default ? Guid.NewGuid() : connectionId;
            IsPublic = ispublic;
            Xchange = exchangeDescriptor.SocketDescriptor.Xchange;
            SocketUrl = exchangeDescriptor.SocketDescriptor.SocketUrl;           
            KeepAliveInterval = TimeSpan.FromSeconds(exchangeDescriptor.SocketDescriptor.KeepAliveInterval);
            _isAuthUrl = !string.IsNullOrEmpty(exchangeDescriptor.SocketDescriptor.AuthUrl);
            _signRequestService = signService;
            var subscriptions = subscriptionTags is null ? exchangeDescriptor.SocketDescriptor.CreatePrivateSubscriptions() : 
                                                           exchangeDescriptor.SocketDescriptor.CreatePublicSubscriptions(subscriptionTags);

            foreach (var s in subscriptions)                            
                Subscriptions.Add(s.Topic, s);
            
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
            try
            {
                await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None).ConfigureAwait(false);
                    
            }
            catch 
            {
            }
            
            Task.Run(async() => await StartListenTcp().ConfigureAwait(false));
            var request = InitOnConnected();
            if (!initialized && request != default)
                await SendRequestAsync(request, CancellationToken.None).ConfigureAwait(false);


        }

        private ClientWebSocket CreateClientWebSocket()
        {
            var client = new ClientWebSocket();            
            client.Options.KeepAliveInterval = KeepAliveInterval;            
            return client;
        }

        private void SetStatus(ConnectionStatus p)
            => Status = p;

        private async Task OnDisconnected()
        {
            ResetSubscriptions();
            SetStatus(ConnectionStatus.DisConnected);
            await StartConnectionAsync();
            SetStatus(ConnectionStatus.Connected);
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
                        await OnDisconnected().ConfigureAwait(false);
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

            await OnDisconnected().ConfigureAwait(false);
        }

        /// <summary>
        /// Method to Send a SocketRequest, is only send if socket is open
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
      
        private async Task SendRequestAsync(SocketRequest request, CancellationToken token)
        {

            if (_clientWebSocket?.State != WebSocketState.Open)
                return;

            TaskCompletionSource<SocketInvocationResult> task = new TaskCompletionSource<SocketInvocationResult>();
            var reqInvocation = new RequestInvocation(request.Id, request, task);
            new CancellationTokenSource(1000 * 120).Token.Register(() =>
            {
                _requestInvocations.TryTake(out reqInvocation);
                task.SetException(new TimeoutException($"Timeout, {Id}: did not receive a response to request {request.Id}"));
            });
            _requestInvocations.Add(reqInvocation);
            var encodedMessage = Encoding.UTF8.GetBytes(request.ToSocketRequestDto());
            await _clientWebSocket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage,
                                                                           offset: 0,
                                                                            count: encodedMessage.Length),
                                        messageType: WebSocketMessageType.Text,
                                       endOfMessage: true,
                                  cancellationToken: token).ConfigureAwait(false);
        }
        /// <summary>
        /// method to remove requestinvocation
        /// TODO: id has to be changed recognized
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest OnCtrlMessage(SocketMessageDto message)
        {
                       
            var invocationFound = _requestInvocations.FirstOrDefault();
            bool requestSuccess = message.Success.GetValueOrDefault(false);
            if (invocationFound != null)
            {

                _requestInvocations.TryTake(out invocationFound);
                if (invocationFound.Request.Method == SocketMethod.Subscribe)
                {
                    //
                    var subscription = Subscriptions.Values.FirstOrDefault(s=>s.Id == invocationFound.Request.Id);
                    if (subscription != null)
                        subscription.IsSubscribed = requestSuccess;
                }

            }
            else 
            {
                _logger.LogWarning($"socket request invocation not fount, message: \n{message.Dump()}");
                throw new InvalidOperationException("request invocation not found");
            }
            if (requestSuccess)
            {
                var newRequest = CreateSubscriptionRequestFromInactive();
                if (newRequest == null)
                    SetStatus(ConnectionStatus.Subscribed);
                return newRequest;
            }
            //throw new SocketRequestException(Id, $"SocketRequest failed, please file the response:\n{message.Dump()}");
            return default;
            
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

        #region CreateRequests
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
            => subscription != null ? CreateNewRequest(SocketMethod.Subscribe, new string[] { subscription.Topic }) : null;

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateUnsubscribeRequest(SocketSubscription subscription)
            => CreateNewRequest(SocketMethod.Unsubscribe, new string[] { subscription.Topic });
        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateAuthRequest()
            => CreateNewRequest(SocketMethod.Auth, _signRequestService.CreatSocketAuthString(Id));                   

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateNewRequest(SocketMethod method, string[] requestString) 
            => new SocketRequest(method, requestString);                              
      
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal void ResetSubscriptions()
            => Subscriptions.Values.Each(s => s.IsSubscribed = false);
        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal SocketSubscription GetInActiveSubscription()
            => Subscriptions.Values.FirstOrDefault(s => s.IsSubscribed == false) == null ? null : Subscriptions.FirstOrDefault(s => s.Value.IsSubscribed == false).Value;

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        internal void SetSubscriptionActive(Guid subscriptionId)
            => Subscriptions.Values.FirstOrDefault(s=>s.Id == subscriptionId);

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketSubscription GetSubscription(string topic)
           => Subscriptions[topic];


        #endregion CreateRequests
        
        /// <summary>
        /// fetches appropriate subscription to fetch the type, deserialize the payload and passes a formatted data packet to
        /// the connection data handler
        /// the deserialization should be abstracted in its own module, todo...
        /// </summary>
        /// <param name="socketMessage"></param>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveOptimization)]
        private Task HandleDataMessage(SocketMessageDto socketMessage) 
        {
            SocketSubscription subscription = GetSubscription(socketMessage.Topic);
            if (socketMessage.Type != DataType.Snapshot)
            {
                SocketDataLayer laer2 = (SocketDataLayer)TypeSerializer.DeserializeFromString(socketMessage.Data, typeof(SocketDataLayer));
                _clientSocketMessageHandle.HandleDataAsync(new SocketUpdatePack(id: Id,
                                                                           xchange: Xchange,
                                                                            insert: TypeSerializer.DeserializeFromString(laer2.Insert, subscription.ReturnArrayType),
                                                                            update: TypeSerializer.DeserializeFromString(laer2.Update, subscription.ReturnArrayType),
                                                                            delete: TypeSerializer.DeserializeFromString(laer2.Delete, subscription.ReturnArrayType),
                                                                         timestamp: socketMessage.Timestamp));
                return Task.CompletedTask;
            }
            if (subscription.Channel == "OrderBook")
            {
                _clientSocketMessageHandle.HandleDataAsync(new SocketMsgPack(id: Id,
                                                                        xchange: Xchange,
                                                                           data: TypeSerializer.DeserializeFromString(socketMessage.Data, subscription.ReturnArrayType),
                                                                      timestamp: socketMessage.Timestamp));
                return Task.CompletedTask;
            }
            _clientSocketMessageHandle.HandleDataAsync(new SocketMsgPack(id: Id,
                                                                        xchange: Xchange,
                                                                           data: TypeSerializer.DeserializeFromString(socketMessage.Data, subscription.ReturnType),
                                                                      timestamp: socketMessage.Timestamp));
            return Task.CompletedTask;
        }


        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private async Task HandleCtrlMessage(SocketMessageDto message)
        {          
            var socketRequest = OnCtrlMessage(message);
            if (socketRequest != null)
                await SendRequestAsync(socketRequest, CancellationToken.None).ConfigureAwait(false);

            return;
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