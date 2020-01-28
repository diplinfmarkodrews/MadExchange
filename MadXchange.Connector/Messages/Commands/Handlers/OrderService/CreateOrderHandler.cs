using Convey.MessageBrokers;
using MadXchange.Common.Handlers;
using MadXchange.Common.Types;
using MadXchange.Connector.Interfaces;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
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
             
        private readonly IInstrumentCache _cachedInstrument;
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IExchangeOrderServiceClient _orderClient;
        private readonly IBusPublisher _busPublisher;

        //private readonly IBusPublisher _busPublisher;
        public CreateOrderHandler(IBusPublisher busPublisher, IInstrumentCache cachedInstrument, IExchangeOrderServiceClient orderClient, ITracer tracer, ILogger<CreateOrderHandler> logger) 
        {
            
            _logger = logger;
            _tracer = tracer;
            _cachedInstrument = cachedInstrument;
            _orderClient = orderClient;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(CreateOrder command) 
        {
            var span = _tracer.BuildSpan("CreateOrder").Start();
            var instrument = await _cachedInstrument.GetInstrumentAsync(command.Exchange, command.Symbol);
            command.Price = command.Side == OrderSide.Buy ? instrument.AskPrice : instrument.BidPrice;
            var order = await _orderClient.PlaceOrderAsync(command);
            if (order.OrdStatus == OrderStatus.NEW || order.OrdStatus == OrderStatus.PENDINGNEW || order.OrdStatus == OrderStatus.PARTIALLYFILLED)
            {
                var orderPlacedEvent = new Events.OrderPlacedEvent(command, order);     
                //Todo Raise events
            }
            else 
            {
                if (order.OrdStatus == OrderStatus.REJECTED) 
                {
                    var orderRejectedEvent = new Events.OrderRejectedEvent(command, order);
                }
            }
            
        }

    }
}
