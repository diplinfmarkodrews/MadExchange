using Convey.CQRS.Commands;
using MadXchange.Common.Types;
using MadXchange.Connector.Interfaces;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using OpenTracing;
using System.Threading.Tasks;

namespace MadXchange.Connector.Messages.Commands.Handlers
{
    public class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
       
        private readonly ILogger _logger;
        private readonly ITracer _tracer;
        private readonly IExchangeRequestServiceClient _orderClient;
        public CancelOrderHandler(IExchangeRequestServiceClient orderClient, ITracer tracer, ILogger<CancelOrderHandler> log) 
        {
            _tracer = tracer;
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
                
            }
        }
    }
}
