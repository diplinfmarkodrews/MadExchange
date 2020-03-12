using MadXchange.Exchange.Messages.Commands.AccountManager;
using MadXchange.Exchange.Messages.Commands.OrderService;
using ServiceStack;
using ServiceStack.Messaging;
using System;

namespace MadXchange.ClientExecution.Services
{
    public class ConnectionManager : Service, IService
    {
        private readonly IMessageQueueClient _rabbitMqClient;
        public ConnectionManager(IMessageQueueClient messageQueueClient) 
        {
            _rabbitMqClient = messageQueueClient;
        }


        public void SetLeverage(Guid id, string symbol, decimal leverage) 
        {
            var setLeverage = new SetLeverage(Guid.NewGuid(), Exchange.Types.Xchange.ByBit, id, symbol, leverage);
            var msgSetLeverage = _rabbitMqClient.CreateMessage<SetLeverage>(setLeverage);
            _rabbitMqClient.Publish<SetLeverage>(msgSetLeverage);
        }

        public void ClientRegister(ClientRegisterSocket registerClient) 
        {
            var message = _rabbitMqClient.CreateMessage<ClientRegisterSocket>(registerClient);
            _rabbitMqClient.Publish(message);
        }
    }
}
