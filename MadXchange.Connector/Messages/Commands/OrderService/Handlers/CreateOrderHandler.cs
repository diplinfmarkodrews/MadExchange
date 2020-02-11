using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using MadXchange.Connector.Interfaces;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.Extensions.Logging;
using OpenTracing;
using System.Threading.Tasks;

namespace MadXchange.Connector.Messages.Commands.Handlers
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder>
    {
        /// <summary>
        /// Cache must be used to obtain price, repo to store orders
        /// </summary>

        private readonly IOrderCache _orderCache;
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IXchangeCommands _orderClient;
        private readonly IBusPublisher _busPublisher;

        //private readonly IBusPublisher _busPublisher;
        public CreateOrderHandler(IBusPublisher busPublisher, IOrderCache orderCache, IXchangeCommands orderClient, ITracer tracer, ILogger<CreateOrderHandler> logger)
        {
            _logger = logger;
            _tracer = tracer;
            _orderCache = orderCache;
            _orderClient = orderClient;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(CreateOrder command)
        {
            //var instrument = await _cachedInstrument.GetInstrumentAsync(command.Exchange, command.Symbol);
            //command.Price = command.Side == OrderSide.Buy ? instrument.AskPrice : instrument.BidPrice;
            var order = await _orderClient.PlaceOrderAsync(command);
            var spanContext = _tracer.ActiveSpan.Context.ToString();
            if (order is null || order.IsOrderAborted())
            {
                var @orderRejectedEvent = new Events.OrderRejectedEvent(command, order);
                await _busPublisher.PublishAsync(@orderRejectedEvent, spanContext: spanContext);
            }
            else
            {
                var @orderPlacedEvent = new Events.OrderPlacedEvent(command, order);
                await _busPublisher.PublishAsync(@orderPlacedEvent, spanContext: spanContext);
            }
        }
    }
}