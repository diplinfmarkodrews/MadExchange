using Convey.CQRS.Commands;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.OrderService.Handlers
{
    public class UpdateOrderHandler : ICommandHandler<UpdateOrder>
    {

        private readonly IXchangeCommands _orderClient;
        private readonly ILogger _logger;

        public UpdateOrderHandler(IXchangeCommands orderClient, ILogger<UpdateOrderHandler> logger)
        {
            _orderClient = orderClient;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateOrder order)
        {
            var update = await _orderClient.UpdateOrderAsync(order);
            if (update is null)
            {
                _logger.LogError($"{order.OrderId}");
                throw new System.Exception("order update failed");
            }
            else
            {
                
            }
        }
    }
}