using MadXchange.Common.Handlers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace MadXchange.Exchange.Handlers.Equity
{
    public class EquityQueryResponseHandler : IEventHandler<EquityRequestResponseEvent>
    {

        private readonly IBusPublisher _busPublisher;
        private readonly ILogger _log;
        public EquityQueryResponseHandler(IBusPublisher publisher, ILogger<EquityQueryResponseHandler> logger)
        {
            _busPublisher = publisher;
            _log = logger;
        }

        public async Task HandleAsync(EquityRequestResponseEvent @event)
        {
            //await _busPublisher.PublishAsync<RestResponseEvent>(@event, context);
            _log.LogTrace("RestRequest successful", @event);
            return;
        }

    }

}
