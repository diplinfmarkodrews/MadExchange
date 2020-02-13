using MadXchange.Connector.Interfaces;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Models.Dto;
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
        void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions, Guid id = default);
    }

    public class SocketConnectionService : ISocketConnectionService
    {
        private readonly ILogger _logger;
        private readonly ISocketConnectionStore _connectionStore;
        private readonly IXchangeDescriptorService _exchangeDescriptorService;
        private readonly IApiKeySetStore _apiKeySetStore;
        private readonly ISignRequests _signService;

        public SocketConnectionService(ISignRequests signService, ISocketConnectionStore connectionStore, IXchangeDescriptorService exchangeDescriptorService, ILogger log)
        {
            _logger = log;
            _connectionStore = connectionStore;
            _exchangeDescriptorService = exchangeDescriptorService;
            _signService = signService;
        }

        public void RegisterSocketClient(Guid accountId, IEnumerable<SocketSubscription> subscritions)
        {
            if (!_connectionStore.Contains(accountId))
            {
                var exchange = _apiKeySetStore.XchangeOf(accountId);
                var exchangeSocketDescriptor = _exchangeDescriptorService.GetSocketDescriptor(exchange);
                var socketConnection = new WebSocketConnection(exchangeSocketDescriptor, subscritions, _signService, connectionId: accountId, isPublic: false);
                _connectionStore.AddSocketConnection(socketConnection);               
            }
            Task.Run(async () => await _connectionStore.GetConnection(accountId).StartConnectionAsync().ConfigureAwait(false));          
        }

        
        public void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions, Guid id = default)
        {
            var socketDescriptor = _exchangeDescriptorService.GetSocketDescriptor(exchange);
            var connection = new WebSocketConnection(socketDescriptor, subscribtions, _signService, connectionId: id, isPublic: true);
            _connectionStore.AddSocketConnection(connection);
        }        

        private string GetConnectionString(Xchange exchange) => _exchangeDescriptorService.GetSocketConnectionString(exchange);

        public void DeRegisterSocketClient(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}