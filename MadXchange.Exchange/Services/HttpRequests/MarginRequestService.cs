using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Services.RequestExecution;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IMarginRequestService
    {
        Task<MarginDto[]> GetMarginAsync(Guid accountId, Exchanges exchange, string currency, CancellationToken token);
    }

    public class MarginRequestService : IMarginRequestService
    {

        private readonly ILogger _logger;
        private readonly IExchangeDescriptorService _descriptorService;
        private readonly IRestRequestService _restRequestService;
        private const string _domainString = "Margin";
        public MarginRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService, ILogger<MarginRequestService> logger)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        public async Task<MarginDto[]> GetMarginAsync(Guid accountId, Exchanges exchange, string currency, CancellationToken token = default) 
        {
          
            var route = _descriptorService.GetExchangeEndPoint(exchange, "GET"+_domainString);
            if (route is null) throw new InvalidOperationException($"endpoint was not found for {exchange} Get: {_domainString}");
            var parameter = string.Empty;
            if (currency != string.Empty)
            {
                parameter = parameter.AddQueryParam(route.Parameter[0], currency);
            }
            var res = await _restRequestService.SendGetAsync(accountId, route.Url, parameter, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<MarginDto[]>(res.Result);

            return result;
        }
    }

    
}
