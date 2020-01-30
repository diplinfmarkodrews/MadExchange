using MadXchange.Common.Handlers;
using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.Handlers
{
    public class SetLeverageHandler : ICommandHandler<SetLeverage>
    {
        private readonly IExchangeRequestServiceClient _orderServiceClient;
        private readonly ILogger _logger;
        public SetLeverageHandler(IExchangeRequestServiceClient orderServiceClient, ILogger<SetLeverageHandler> logger) 
        {
            _orderServiceClient = orderServiceClient;
            _logger = logger;
        }
        public async Task HandleAsync(SetLeverage leverage) 
        {
        
        }
    }
}
