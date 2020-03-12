using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Messages.Events;
using Microsoft.Extensions.Logging;
using ServiceStack.Text;
using System.Threading.Tasks;
using ITracer = OpenTracing.ITracer;

namespace MadXchange.Exchange.Messages.Commands.OrderService.Handlers
{
    public class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IXchangeCommands _orderClient;
        private readonly IBusPublisher _busPublisher;

        public CancelOrderHandler(IBusPublisher publisher, IXchangeCommands orderClient, ITracer tracer, ILogger<CancelOrderHandler> log)
        {
            _tracer = tracer;
            _orderClient = orderClient;
            _busPublisher = publisher;
            _logger = log;
        }

        public async Task HandleAsync(CancelOrder command)
        {
            var res = await _orderClient.CancelOrderAsync(command);
            if (res is null)
            {
                _logger.LogError($"OrderService client response is null{command.Dump()}", command);
                await _busPublisher.PublishAsync(new CancelOrderRejectedEvent(command: command));
            }
            else
            {
                await _busPublisher.PublishAsync(new CancelOrderEvent(command: command));
            }
        }
    }
}