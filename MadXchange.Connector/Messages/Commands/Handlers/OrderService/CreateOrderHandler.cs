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
             
        private readonly ICachedPriceService _cachedPriceService;
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IExchangeOrderServiceClient _orderClient;
        private readonly IBusPublisher _busPublisher;

        //private readonly IBusPublisher _busPublisher;
        public CreateOrderHandler(IBusPublisher busPublisher, ICachedPriceService cachedPriceService, IExchangeOrderServiceClient orderClient, ITracer tracer, ILogger<CreateOrderHandler> logger) 
        {
            
            _logger = logger;
            _tracer = tracer;
            _cachedPriceService = cachedPriceService;
            _orderClient = orderClient;

        }

        public async Task HandleAsync(CreateOrder command) 
        {
            var instrument = await _cachedPriceService.GetPriceAsync(command.ExchangeId, command.Symbol, command.Side);
            command.Price = command.Side == OrderSide.Buy ? instrument.AskPrice : instrument.BidPrice;
            var order = await _orderClient.PlaceOrderAsync(command);
            if (order.OrdStatus == OrderStatus.NEW || order.OrdStatus == OrderStatus.PENDINGNEW || order.OrdStatus == OrderStatus.PARTIALLYFILLED)
            {
                var orderPlacedEvent = new Events.OrderPlacedEvent(command, order);     
                
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
