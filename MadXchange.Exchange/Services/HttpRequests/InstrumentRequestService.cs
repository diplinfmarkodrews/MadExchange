using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using MadXchange.Exchange.Dto.Http;
using MadXchange.Exchange.Types;
using MadXchange.Exchange.Services.RequestExecution;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IInstrumentRequestService 
    {
        Task<InstrumentDto[]> GetInstrumentAsync(Exchanges exchange, string symbol);
    
    }
    public class InstrumentRequestService : IInstrumentRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
        public InstrumentRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService, ILogger<InstrumentRequestService> logger) 
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }
        public async Task<InstrumentDto[]> GetInstrumentAsync(Exchanges exchange, string symbol) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            ///Todo: construction of url goes into exchangedescriptor service
            ///exchangedescriptor service provides for routing attributes and datacontracts for return types
            ///the (domaindata)requestservice adapts the requests and result appropriately
            var route = descriptor.RouteGetInstrument;
            string url = $"{descriptor.BaseUrl}{route.Url}";
            if(symbol != string.Empty)
            {
                url = url.AddQueryParam(route.Parameter[0].Param.Value, symbol);
            }                        
            var res = await _restRequestService.SendGetAsync(url).ConfigureAwait(false);
            //Mapping
            //route.R
            var result = res.result.ConvertTo<InstrumentDto[]>();
            result.Each(f => f.Exchange = exchange);
            return result;//.FromJson<Instrument>();

        }
    }
}
