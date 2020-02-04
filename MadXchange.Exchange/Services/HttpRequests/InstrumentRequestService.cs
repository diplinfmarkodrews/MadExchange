using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Services.RequestExecution;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;
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
            
        }

        public async Task<InstrumentDto[]> GetInstrumentAsync(Exchanges exchange, string symbol) 
        {

            var endPoint = _descriptorService.GetExchangeEndPoint(exchange, "GETInstrument"); //=> both to Dictionary=> only 1access 
            var url = endPoint.Url;
            if (symbol != string.Empty)
            {
                url = url.AddQueryParam(endPoint.Parameter[0].Param.Item1, symbol);
            }                        
            var res = await _restRequestService.SendGetAsync(url).ConfigureAwait(false);            
            var result = TypeSerializer.DeserializeFromString<InstrumentDto[]>(res.Result); //res.result.ConvertTo<InstrumentDto[]>();
            result.Each(f => f.Exchange = exchange);
            return result;

        }
    }
}
