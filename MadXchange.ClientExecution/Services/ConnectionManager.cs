using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers;
using ServiceStack; 
namespace MadXchange.ClientExecution.Services
{
    public class ConnectionManager : IService
    {
        private readonly IBusPublisher _busPublisher;
        public ConnectionManager(IBusPublisher busPublisher) 
        {
            _busPublisher = busPublisher;
        }
        public async Task OnClientRegister(ClientRegisterSocket registerClient) 
        {
            await _busPublisher.PublishAsync(registerClient, "myId").ConfigureAwait(false);
        }
    }
}
