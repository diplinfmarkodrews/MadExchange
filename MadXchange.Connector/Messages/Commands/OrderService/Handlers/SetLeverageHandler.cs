using Convey.CQRS.Commands;
using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Messages.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.Handlers
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