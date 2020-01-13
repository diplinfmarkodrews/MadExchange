using MadXchange.Common.Handlers;
using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using OpenTracing;
using System.Threading.Tasks;


namespace MadXchange.Exchange.Messages.Commands.Handlers
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder>
    {

        /// <summary>
        /// Cache must be used to obtain price, repo to store orders
        /// </summary>
        private readonly IAsyncRepository<IOrder> _repository;        
        private readonly ICachedInstrumentService _instrumentService;
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IExchangeOrderServiceClient _orderClient;
        //private readonly IBusPublisher _busPublisher;
        public CreateOrderHandler(ICachedInstrumentService instrument, IExchangeOrderServiceClient orderClient, IAsyncRepository<IOrder> repo, ITracer tracer, ILogger<CreateOrderHandler> logger) 
        {
            _repository = repo;
            _logger = logger;
            _tracer = tracer;
            _instrumentService = instrument;
            _orderClient = orderClient;

        }

        public async Task HandleAsync(CreateOrder command) 
        {
            command.Price = await _instrumentService.GetPriceAsync(command.ExchangeId, command.Symbol, command.Side);             
            var order = await _orderClient.PlaceOrderAsync(command);
            if (order.OrdStatus == OrderStatus.NEW || order.OrdStatus == OrderStatus.PARTIALLYFILLED)
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
            await _repository.Add(order);
        }

    }
}
