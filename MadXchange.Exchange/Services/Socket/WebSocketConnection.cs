using MadXchange.Common;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Exceptions;
using MadXchange.Exchange.Helpers;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Types;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
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

        private class RequestInvocation : IDisposable
        {
            public string RequestKey { get; } 
            public SocketRequest Request { get; set; }
            public TaskCompletionSource<SocketInvocationResult> RequestCompletionSource { get; set; }
            public RequestInvocation(string requestKey, SocketRequest request, TaskCompletionSource<SocketInvocationResult> task) 
            {
                RequestKey = requestKey;
                Request = request;
                RequestCompletionSource = task;
                //CancellationTokenSource cancellationSource
            }
            public void Dispose()
            {
                ((IDisposable)Request).Dispose();
            }
        }

        public Guid Id { get; }
        public Xchange Xchange { get; }
        public bool IsPublic { get; } 
        public Dictionary<string, SocketSubscription> Subscriptions { get; } = new Dictionary<string, SocketSubscription>();        
        public ClientWebSocket Socket { get; set; }

        internal void OnDisconnected()
        {
            ResetSubscriptions();
            SetStatus(ConnectionStatus.DisConnected);
        }

        public ClientWebSocketOptions Options { get; set; }
        public string SocketUrl { get; }
        public ConnectionStatus Status { get; internal set; } = ConnectionStatus.Offline;
        private XchangeSocketDescriptor _exchangeSocketDescriptor { get; }
        
        
        private readonly ISignRequests _signRequestService;
        private bool _isAuthUrl;
        private Dictionary<string, RequestInvocation> _requestInvocations = new Dictionary<string, RequestInvocation>();
        public WebSocketConnection(XchangeSocketDescriptor exchangeSocketDescriptor, IEnumerable<SocketSubscription> subscriptions, ISignRequests signService, Guid connectionId = default, bool isPublic = true)
        {

            _exchangeSocketDescriptor = exchangeSocketDescriptor;
            Id = connectionId == default ? Guid.NewGuid() : connectionId;            
            Xchange = _exchangeSocketDescriptor.Xchange;
            SocketUrl = _exchangeSocketDescriptor.SocketUrl;
            IsPublic = isPublic;
            Options.KeepAliveInterval = TimeSpan.FromSeconds(_exchangeSocketDescriptor.KeepAliveInterval);
            _isAuthUrl = !string.IsNullOrEmpty(_exchangeSocketDescriptor.AuthUrl);
            _signRequestService = signService;
            foreach (var s in subscriptions)
                Subscriptions.Add(s.Topic, s); 
            
            
        }

        [MethodImpl(methodImplOptions:MethodImplOptions.AggressiveInlining)]
        private ClientWebSocket CreateClientWebSocket()
        {

            var client = new ClientWebSocket();
            client.Options.KeepAliveInterval = Options.KeepAliveInterval;
            return client;

        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private void SetStatus(ConnectionStatus p) => Status = p;

        public async Task StartConnectionAsync()
        {
            // also check if connection was lost, that's probably why we get called multiple times.
            if (Socket == null || Socket.State != WebSocketState.Open)
            {
                // create a new web-socket so the next connect call works.
                Socket?.Dispose();
                Socket = CreateClientWebSocket();
            }
            // don't do anything, we are already connected.
            else return;
            var url = SocketUrl;
            if (!IsPublic)
                url = _signRequestService.SignSocketUrl(Id, SocketUrl);
            await Socket.ConnectAsync(new Uri(url), CancellationToken.None).ConfigureAwait(false);
        }




        [MethodImpl(methodImplOptions:MethodImplOptions.AggressiveInlining)]
        internal SocketRequest OnCtrlMessage(SocketResponse message)
        {
           
            RequestInvocation request = default;
            string topic = message.GetTopic();
            var invocationFound = _requestInvocations.Remove(message.Method.ToString() + topic, out request);
            if (invocationFound)
            {
                bool requestSuccess = message.IsSuccess();
                request.RequestCompletionSource.TrySetResult(new SocketInvocationResult() { SocketMethod = message.Method, 
                                                                                              RequestKey = request.RequestKey, 
                                                                                                  Result = requestSuccess });                
                if (message.Method == SocketMethod.Subscribe)
                {
                    var subscription = Subscriptions.GetValueOrDefault(topic);
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
        internal SocketRequest InitOnConnected() 
        {
            Status = ConnectionStatus.Connected;
            ResetSubscriptions();
            if (!(IsPublic && _isAuthUrl))
                return CreateAuthRequest();            
            return CreateSubscriptionRequestFromInactive();
        }

        /// <summary>
        /// Connection generates SocketRequest object from predefined objectDictionaries. 
        /// can be optimized later on. but now it gives us more flexibility.
        /// 
        /// - CreateSubscriptionFromInactive: incremently goes through all subscriptions of a connection and activates it.
        /// - foreach request type there is afunction to create it.
        /// - all functions are mapped to create new, to setup invocationHandle, a callback to register requests and a timeout
        /// </summary>
        /// <returns></returns>
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateSubscriptionRequestFromInactive()
            => CreateSubscriptionRequest(GetInActiveSubscription().Request);
        
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateSubscriptionRequest(ObjectDictionary request)
            => CreateNewRequest(SocketMethod.Subscribe, request);
        

        [MethodImpl(methodImplOptions:MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateAuthRequest()
        {
            ObjectDictionary authRequest =  _signRequestService.CreatSocketAuthRequest(Id);
            return CreateNewRequest(SocketMethod.Unsubscribe, authRequest);
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private SocketRequest CreateUnsubscribeRequest(SocketSubscription subscription)
            => CreateNewRequest(SocketMethod.Unsubscribe, subscription.Request);
        
        private SocketRequest CreateNewRequest(SocketMethod method, ObjectDictionary requestDict) 
        {
            var request = new SocketRequest(method, requestDict);
            string topic = request.GetTopic?.Invoke();
            topic = $"{request.Method.ToString()}{topic}";
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
        internal SocketSubscription GetInActiveSubscription() => Subscriptions.Values.FirstOrDefault(s => s.IsSubscribed == false);
        internal void SetSubscriptionActive(Guid subscriptionId) => Subscriptions.Values.FirstOrDefault(s=>s.Id == subscriptionId);
        
        internal Task OnData(SocketMessage socketMessage) 
        {
            SocketSubscription subscription = socketMessage.GetSubscription(Subscriptions);

            var type = subscription.ReturnType;
            //object  result = TypeSerializer.DeserializeFromString(type, socketMessage.Data);
            return Task.CompletedTask;
        }
        

        private async Task ReconnectAsync() 
        {
            Status = ConnectionStatus.DisConnected;
            Socket.Abort();           
            await StartConnectionAsync();
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
                    Socket?.Abort();
                    Socket = null;
                    Socket.Dispose();
                    Subscriptions.Clear();
                    
                }

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