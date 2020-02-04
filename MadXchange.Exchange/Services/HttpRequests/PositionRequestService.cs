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

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IPositionRequestService
    {
    }

    public class PositionRequestService : IPositionRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
        private const string _domainString = "Position";
        public PositionRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService, ILogger<PositionRequestService> logger)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        public async Task<PositionDto[]> GetPositionAsync(Guid accountId, Exchanges exchange, string symbol, CancellationToken token = default)
        {
            var route = _descriptorService.GetExchangeEndPoint(exchange, _domainString);
            var url = _domainString;///Todo fix!
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            var res = await _restRequestService.SendGetAsync(accountId, url, parameter, token).ConfigureAwait(false);            
            return res.Result.FromJson<PositionDto[]>();
        }
        /// <summary>
        /// Get Leverage
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LeverageDto>> GetLeverageAsync(Guid accountId, Exchanges exchange, string symbol, CancellationToken token = default)
        {

            var route = _descriptorService.GetExchangeEndPoint(exchange, symbol);
            var url = _domainString;
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            var res = await _restRequestService.SendGetAsync(accountId, url, parameter, token).ConfigureAwait(false);                        
            return res.Result.FromJson<IEnumerable<LeverageDto>>();
        }

        public async Task<IEnumerable<LeverageDto>> PostLeverageAsync(Guid accountId, Exchanges exchange, string symbol, decimal leverage, CancellationToken token = default)
        {

            var route = _descriptorService.GetExchangeEndPoint(exchange, symbol);
            var url = _domainString;
            var parameter = string.Empty;            
            parameter.AddQueryParam(route.Parameter[0], symbol);                            
            parameter.AddQueryParam(route.Parameter[1], leverage.ToString());
            var res = await _restRequestService.SendGetAsync(accountId, url, parameter, token).ConfigureAwait(false);
            return res.Result.FromJson<IEnumerable<LeverageDto>>();
        }

    }

    
}
