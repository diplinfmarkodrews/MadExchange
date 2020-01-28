using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto.Http;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IInstrumentRequestService 
    {
        Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol);
    
    }
    public class InstrumentRequestService : IInstrumentRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
        public InstrumentRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService,  ILogger<InstrumentRequestService> logger) 
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }
        public async Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteGetInstrument;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if(symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }                        
            var res = await _restRequestService.SendGetAsync<WebResponseDto>(url).ConfigureAwait(false);
            //Mapping
            var result = res.Result.ConvertTo<Instrument>();
            return result;//.FromJson<Instrument>();

        }
    }
}
