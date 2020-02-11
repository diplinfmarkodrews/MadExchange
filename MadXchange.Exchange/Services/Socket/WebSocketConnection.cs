using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Services.Utils;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
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
        public bool IsPublic { get; } = false;
        public Dictionary<Guid, SocketSubscription> Subscriptions { get; } = new Dictionary<Guid, SocketSubscription>();        
        public ClientWebSocket Socket { get; set; }
        public ClientWebSocketOptions Options { get; set; }
        public string BaseUrl { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; } = ConnectionStatus.Offline;
        private readonly ISignRequests _signRequestService;
        
        public WebSocketConnection(Xchange exchange, string url, IEnumerable<SocketSubscription> subscriptions, ISignRequests signService, Guid connectionId = default, bool isPublic = false)
        {
            Id = connectionId == Guid.Empty ? Guid.NewGuid() : connectionId;
            Xchange = exchange;
            BaseUrl = url;
            IsPublic = isPublic;
            _signRequestService = signService;
            foreach (var s in subscriptions)
                Subscriptions.Add(s.Id, s);
                                    
        }
        private ClientWebSocket CreateClientWebSocket() 
        {
            
            var client = new ClientWebSocket();
            client.Options.KeepAliveInterval = TimeSpan.FromSeconds(60);
            return client;
        
        }
        public void SetStatus(ConnectionStatus p) => ConnectionStatus = p;        

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
            var url = BaseUrl;
            if (!IsPublic)
                url = _signRequestService.SignSocketUrl(Id, BaseUrl);
            await Socket.ConnectAsync(new Uri(url), CancellationToken.None).ConfigureAwait(false);
        }
        
        

        private async Task ReconnectAsync() 
        {
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