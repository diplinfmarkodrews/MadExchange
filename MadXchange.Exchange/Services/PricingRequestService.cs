using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IPricingRequestService 
    {
        Task<IInstrument> GetInstrument(Exchanges exchange, string symbol);
    
    }
    public class PricingRequestService : IPricingRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
        public PricingRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService,  ILogger<PricingRequestService> logger) 
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }
        public async Task<IInstrument> GetInstrument(Exchanges exchange, string symbol) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteGetInstrument;
            var url = $"{descriptor.BaseUrl}/{route.Url}".AddQueryParam(route.Parameter[0], symbol);
            

            
            var res = await _restRequestService.SendGetAsync(exchange, url).ConfigureAwait(false);
            //Mapping
            return res.FromJson<Instrument>();

        }
    }
}
