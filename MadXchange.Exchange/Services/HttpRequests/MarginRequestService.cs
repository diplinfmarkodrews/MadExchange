using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.XchangeDescriptor;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IMarginRequestService
    {
        Task<MarginDto[]> GetMarginAsync(Guid accountId, Xchange exchange, string currency, CancellationToken token);
    }

    public class MarginRequestService : IMarginRequestService
    {
        private readonly ILogger _logger;
        private readonly IXchangeDescriptorService _descriptorService;
        private readonly IRestRequestService _restRequestService;
        private const string _currencyString = "Currency";

        public MarginRequestService(IXchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService, ILogger<MarginRequestService> logger)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        public async Task<MarginDto[]> GetMarginAsync(Guid accountId, Xchange exchange, string currency, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.GetMargin, new ObjectDictionary() { { _currencyString, currency } });
            var response = await _restRequestService.SendRequestObjectAsync(accountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<MarginDto[]>(response.Result);
            result.Each(p => 
            {   
                p.AccountId = accountId; 
                p.Exchange = exchange; 
                p.Timestamp = response.Timestamp; 
            });
            return result;

        }
    }
}