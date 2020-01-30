using Convey.CQRS.Commands;
using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.Handlers
{
    public class SetLeverageHandler : ICommandHandler<SetLeverage>
    {
        private readonly IExchangeRequestService _orderServiceClient;
        private readonly ILogger _logger;
        public SetLeverageHandler(IExchangeRequestService orderServiceClient, ILogger<SetLeverageHandler> logger) 
        {
            _orderServiceClient = orderServiceClient;
            _logger = logger;
        }
        public async Task HandleAsync(SetLeverage leverage) 
        {
        
        }
    }
}
