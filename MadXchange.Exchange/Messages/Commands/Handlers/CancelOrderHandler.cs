using MadXchange.Common.Handlers;
using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using OpenTracing;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.Handlers
{
    public class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
        private readonly IAsyncRepository<IOrder> _orderRepository;
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IExchangeOrderServiceClient _orderClient;
        public CancelOrderHandler(IAsyncRepository<IOrder> orderRepository, IExchangeOrderServiceClient orderClient, ILogger<CancelOrderHandler> log) 
        {
            _orderRepository = orderRepository;
            _orderClient = orderClient;
            _logger = log;
        
        }
        public async Task HandleAsync(CancelOrder command) 
        {
            
            var res = await _orderClient.CancelOrderAsync(command);
            if (res is null) 
            {
                _logger.LogError("OrderService client response is null", command);
            }
            else 
            {
                await _orderRepository.Update(res);
            }
        }
    }
}
