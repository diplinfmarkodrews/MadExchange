using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadXchange.Connector.Messages.Commands.AccountManager;
using ServiceStack;
using ServiceStack.Messaging;

namespace MadXchange.ClientExecution.Services
{
    public class ConnectionManager : IService
    {
        private readonly IMessageQueueClient _rabbitMqClient;
        public ConnectionManager(IMessageQueueClient messageQueueClient) 
        {
            _rabbitMqClient = messageQueueClient;
        }
        public void OnClientRegister(ClientRegisterSocket registerClient) 
        {
            var message = _rabbitMqClient.CreateMessage<ClientRegisterSocket>(registerClient);
            _rabbitMqClient.Publish("myId", message);
        }
    }
}
