using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.XchangeDescriptor;
using MadXchange.Exchange.Types;
using ServiceStack;
using ServiceStack.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IInstrumentRequestService
    {
        Task<InstrumentDto[]> GetInstrumentAsync(Xchange exchange, string symbol);
    }

    public class InstrumentRequestService : IInstrumentRequestService
    {
        private IXchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;

        public InstrumentRequestService(IXchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
        }

        public async Task<InstrumentDto[]> GetInstrumentAsync(Xchange exchange, string symbol = default) // wont be passed atm
        {            
            var requestDictionary = _descriptorService.GetPublicEndPointUrl(exchange, XchangeHttpOperation.GetInstrument); //
            var res = await _restRequestService.SendGetAsync(requestDictionary).ConfigureAwait(false);         
            var result = TypeSerializer.DeserializeFromString<InstrumentDto[]>(res.Result);
            result.Each(f =>
            {
                f.Exchange = exchange;
                f.Timestamp = res.Timestamp;                
            });
            return result;
          
        }
    }
}