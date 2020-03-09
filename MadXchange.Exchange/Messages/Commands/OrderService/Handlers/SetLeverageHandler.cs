using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using MadXchange.Exchange.Interfaces;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.OrderService.Handlers
{
    public class SetLeverageHandler : ICommandHandler<SetLeverage>
    {
        private readonly IXchangeCommands _orderServiceClient;
        private readonly ILogger _logger;

        public SetLeverageHandler(IXchangeCommands orderServiceClient, ILogger<SetLeverageHandler> logger)
        {
            _orderServiceClient = orderServiceClient;
            _logger = logger;
        }

        public async Task HandleAsync(SetLeverage leverage)
        {
        }
    }
}