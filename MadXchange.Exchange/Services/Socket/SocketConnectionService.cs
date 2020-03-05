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
        void RegisterSocketClient(Guid accountId);
        void DeRegisterSocketClient(Guid accountId);
        void RegisterPublicSocket(Xchange exchange, (string, string)[] subscribtions, Guid id = default);
        void DeRegisterSocketInstrument(Xchange exchange, Guid id);
    }

    public class SocketConnectionService : ISocketConnectionService
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly OpenTracing.ITracer _tracer;
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
                                          OpenTracing.ITracer tracer,
                                               ILoggerFactory log)
        {
            _logger = log.CreateLogger<SocketConnectionService>();
            _loggerFactory = log;
            _tracer = tracer;
            _apiKeySetStore = keySetStore;
            _connectionStore = connectionStore;
            _exchangeDescriptorService = exchangeDescriptorService;
            _signService = signService;
            _clientSocketDataHandler = clientSocketDataHandler;
        }
        

        public void RegisterSocketClient(Guid accountId)
        {
                      
            _logger.LogInformation($"adding client websocket connection, id: {accountId}");
            WebSocketConnection webSocketConnection;
            if (!_connectionStore.Contains(accountId))
            {
                
                webSocketConnection = new WebSocketConnection(socketMessageHandler: _clientSocketDataHandler, 
                                                                exchangeDescriptor: _exchangeDescriptorService.GetXchangeDescriptor(_apiKeySetStore.XchangeOf(accountId)),                                                                       
                                                                       signService: _signService,
                                                                            tracer: _tracer,
                                                                            logger: _loggerFactory.CreateLogger<WebSocketConnection>(),
                                                                      connectionId: accountId,                                                                                 
                                                                          ispublic: false);

                _connectionStore.AddSocketConnection(webSocketConnection);
               
            }
            else            
                webSocketConnection = _connectionStore.GetConnection(accountId);        
            
            Task.Run(async() => await webSocketConnection.StartConnectionAsync().ConfigureAwait(false));
            _logger.LogInformation($"client websocket connection added, id: {accountId}");
        }

        

        public void RegisterPublicSocket(Xchange exchange, (string, string)[] subscribtions, Guid id = default)
        {
            _logger.LogInformation($"adding public websocket connection, Exchange: {exchange} \n subscriptions: {subscribtions}");
            var connection = new WebSocketConnection(socketMessageHandler: _clientSocketDataHandler, 
                                                       exchangeDescriptor: _exchangeDescriptorService.GetXchangeDescriptor(exchange),                                                           
                                                              signService: _signService,
                                                                   tracer: _tracer,
                                                                   logger: _loggerFactory.CreateLogger<WebSocketConnection>(),
                                                         subscriptionTags: subscribtions,
                                                             connectionId: id, 
                                                                 ispublic: true);

            _connectionStore.AddSocketConnection(connection);       
            
            Task.Run(async() => await connection.StartConnectionAsync()).ConfigureAwait(false);
            _logger.LogInformation($"public websocket connection added, Exchange: {exchange} has id: {connection.Id}");
        }        

 
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
