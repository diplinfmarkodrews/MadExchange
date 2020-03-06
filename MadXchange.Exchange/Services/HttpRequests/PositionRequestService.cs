using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.XchangeDescriptor;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests
{
    public interface IPositionRequestService
    {
        Task<PositionDto> GetPositionAsync(Guid accountId, Xchange exchange, string symbol, CancellationToken token = default);
        Task<PositionDto[]> GetPositionsAsync(Guid accountId, Xchange exchange, CancellationToken token = default);
        Task<LeverageDto[]> GetLeverageAsync(Guid accountId, Xchange exchange, CancellationToken token = default);
        Task<LeverageDto> PostLeverageAsync(Guid accountId, Xchange exchange, string symbol, decimal leverage, CancellationToken token = default);
    }

    public class PositionRequestService : IPositionRequestService, IService
    {
        private readonly ILogger _logger;
        private readonly IXchangeDescriptorService _descriptorService;
        private readonly IRestRequestService _restRequestService;

        public PositionRequestService(IXchangeDescriptorService exchangeDescriptorService, 
                                            IRestRequestService restRequestService, 
                                ILogger<PositionRequestService> logger)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        public async Task<PositionDto> GetPositionAsync(Guid accountId, Xchange exchange, string symbol, CancellationToken token = default)
        {
            var requestObject = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.GetPosition, new ObjectDictionary() { { IXchangeDescriptorService.SymbolString, symbol } });
            var response = await _restRequestService.SendRequestObjectAsync(accountId, requestObject, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<PositionDto>(response.Result);
            result.AccountId = accountId;            
            result.Exchange = exchange;
            result.Timestamp = result.Timestamp;
            return result;
        }

        public async Task<PositionDto[]> GetPositionsAsync(Guid accountId, Xchange exchange, CancellationToken token = default)
        {
            var requestObject = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.GetPositionList, new ObjectDictionary());
            var response = await _restRequestService.SendRequestObjectAsync(accountId, requestObject, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<PositionDto[]>(response.Result);
            result.Each(p => { p.AccountId = accountId; p.Exchange = exchange; p.Timestamp = response.Timestamp; });
            return result;
        }

        public async Task<LeverageDto[]> GetLeverageAsync(Guid accountId, Xchange exchange, CancellationToken token = default)
        {
            var requestObject = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.GetLeverage, new ObjectDictionary());
            var response = await _restRequestService.SendRequestObjectAsync(accountId, requestObject, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<LeverageDto[]>(response.Result);
            result.Each(p => { p.AccountId = accountId; p.Exchange = exchange; p.Timestamp = response.Timestamp; });
            return result;
        }

        public async Task<LeverageDto> PostLeverageAsync(Guid accountId, Xchange exchange, string symbol, decimal leverage, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.PostLeverage, new ObjectDictionary() { { IXchangeDescriptorService.LeverageString, (int)leverage }, { IXchangeDescriptorService.SymbolString, symbol } });
            var response = await _restRequestService.SendRequestObjectAsync(accountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<LeverageDto>(response.Result);
            result.AccountId = accountId;
            result.Exchange = exchange;
            result.Timestamp = result.Timestamp;
            return result;
        }
    }
}