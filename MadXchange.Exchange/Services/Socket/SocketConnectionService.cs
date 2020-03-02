using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Handler;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Services.XchangeDescriptor;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{
    public interface ISocketConnectionService
    {
        void RegisterSocketClient(Guid accountId, IEnumerable<SocketSubscription> subscritions);
        void DeRegisterSocketClient(Guid accountId);
        void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions, Guid id = default);
        void DeRegisterSocketInstrument(Xchange exchange, Guid id);
    }

    public class SocketConnectionService : ISocketConnectionService
    {
        private readonly ILogger _logger;
        private readonly ISocketConnectionStore _connectionStore;
        private readonly IXchangeDescriptorService _exchangeDescriptorService;
        private readonly IApiKeySetStore _apiKeySetStore;
        private readonly ISignRequestsService _signService;
        private readonly IConnectionDataHandler _clientSocketDataHandler;

        public SocketConnectionService(IConnectionDataHandler clientSocketDataHandler, 
                                         ISignRequestsService signService, 
                                       ISocketConnectionStore connectionStore, 
                                              IApiKeySetStore keySetStore, 
                                    IXchangeDescriptorService exchangeDescriptorService, 
                             ILogger<SocketConnectionService> log)
        {
            _logger = log;
            _apiKeySetStore = keySetStore;
            _connectionStore = connectionStore;
            _exchangeDescriptorService = exchangeDescriptorService;
            _signService = signService;
            _clientSocketDataHandler = clientSocketDataHandler;
        }
        

        public void RegisterSocketClient(Guid accountId, IEnumerable<SocketSubscription> subscritions)
        {
            _logger.LogInformation($"socket connection added, id: {accountId}, subscriptions:\n{subscritions.Dump()}");
            WebSocketConnection webSocketConnection;
            if (!_connectionStore.Contains(accountId))
            {
                
                webSocketConnection = new WebSocketConnection(socketMessageHandler: _clientSocketDataHandler, 
                                                          exchangeDescriptor: _exchangeDescriptorService.GetXchangeDescriptor(_apiKeySetStore.XchangeOf(accountId)), 
                                                                     subscriptions: subscritions, 
                                                                       signService: _signService, 
                                                                      connectionId: accountId, 
                                                                          ispublic: false);

                _connectionStore.AddSocketConnection(webSocketConnection);
               
            }
            else            
                webSocketConnection = _connectionStore.GetConnection(accountId);                       
            Task.Run(async() => await webSocketConnection.StartConnectionAsync().ConfigureAwait(false));           
        }

        

        public void RegisterSocketInstrument(Xchange exchange, IEnumerable<SocketSubscription> subscribtions, Guid id = default)
        {

            var connection = new WebSocketConnection(socketMessageHandler: _clientSocketDataHandler, 
                                                 exchangeDescriptor: _exchangeDescriptorService.GetXchangeDescriptor(exchange), 
                                                            subscriptions: subscribtions, 
                                                              signService: _signService, 
                                                             connectionId: id, 
                                                                 ispublic: true);

            _connectionStore.AddSocketConnection(connection);            
            Task.Run(async() => await connection.StartConnectionAsync()).ConfigureAwait(false);            
        }        

        private string GetConnectionString(Xchange exchange) => _exchangeDescriptorService.GetSocketConnectionString(exchange);

        
        
        
        
        /// <summary>
        /// Deregister SocketClient with accountId
        /// </summary>
        /// <param name="accountId"></param>
        public void DeRegisterSocketClient(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void DeRegisterSocketInstrument(Xchange exchange, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
