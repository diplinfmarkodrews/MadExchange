using MadXchange.Connector.Interfaces;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Services.XchangeDescriptor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{
    public interface ISocketConnectionService
    {
        void RegisterSocketClient(Guid accountId, IEnumerable<SocketSubscription> subscritions);
        void DeRegisterSocketClient(Guid accountId);
        void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions);
    }

    public class SocketConnectionService : ISocketConnectionService
    {
        private readonly ILogger _logger;
        private readonly ISocketConnectionStore _connectionStore;
        private readonly IXchangeDescriptorService _exchangeDescriptorService;
        private readonly IApiKeySetStore _apiKeySetStore;
        private readonly ISignRequests _signService;

        public SocketConnectionService(ISocketConnectionStore connectionStore, IXchangeDescriptorService exchangeDescriptorService, ILogger log)
        {
            _logger = log;
            _connectionStore = connectionStore;
            _exchangeDescriptorService = exchangeDescriptorService;
        }

        public void RegisterSocketClient(Guid accountId, IEnumerable<SocketSubscription> subscritions)
        {

            if (!_connectionStore.Contains(accountId))
            {
                var exchange = _apiKeySetStore.XchangeOf(accountId);
                var connectionString = _exchangeDescriptorService.GetSocketConnectionString(exchange);
                var socketConnection = new WebSocketConnection(exchange, connectionString, subscritions, _signService, connectionId: accountId, isPublic: false);
                _connectionStore.AddSocketConnection(socketConnection);

            }
            
        }

        
        public void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions)
        {
            var socketUrl = GetConnectionString(exchange);
            var client = CreateClient(socketUrl, exchange.ToString());
        }

        

        private string GetConnectionString(Xchange exchange) => _exchangeDescriptorService.GetSocketConnectionString(exchange);

        

        

        private IMadSocketClient CreateClient(string connectionString, string clientId)
        {
            //var socketClient = new SocketClientService(connectionString, clientId);
            //var isConnected = socketClient.Connect();

            return null;//socketClient;
        }

        public void DeRegisterSocketClient(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}