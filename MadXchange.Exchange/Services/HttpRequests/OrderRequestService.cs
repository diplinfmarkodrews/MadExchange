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
    public interface IOrderRequestService
    {
        Task<OrderDto[]> GetOpenOrdersAsync(Xchange exchange, Guid accountId, string symbol, CancellationToken token);
        Task<OrderDto> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token);
        Task<OrderDto> PostUpdateOrderAsync(OrderPutRequestDto request, CancellationToken token);
        Task<OrderDto> DeleteOrderAsync(Xchange exchange, Guid accountId, string symbol, string orderId, CancellationToken token);
        Task<OrderDto[]> DeleteAllOrdersAsync(Xchange exchange, Guid accountId, string symbol, CancellationToken token);
    }

    public class OrderRequestService : IOrderRequestService
    {
        private ILogger _logger;
        private IXchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;

        public OrderRequestService(IXchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService, ILogger<OrderRequestService> logger)
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        public async Task<OrderDto[]> GetOpenOrdersAsync(Xchange exchange, Guid accountId, string symbol, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.GetOrder, new ObjectDictionary() { { IXchangeDescriptorService.SymbolString, symbol } });
            var res = await _restRequestService.SendRequestObjectAsync(accountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto[]>(res.Result);
            result.Each(p => { p.AccountId = accountId; p.Exchange = exchange; p.Timestamp = res.Timestamp; });
            return result;
        }

        public async Task<OrderDto> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(request.Exchange, XchangeHttpOperation.PostPlaceOrder, (ObjectDictionary)request.MergeIntoObjectDictionary());            
            var res = await _restRequestService.SendRequestObjectAsync(request.AccountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
            result.AccountId = request.AccountId;
            result.Exchange = request.Exchange;
            result.Timestamp = result.Timestamp;
            return result;
        }

        public async Task<OrderDto> PostUpdateOrderAsync(OrderPutRequestDto request, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(request.Exchange, XchangeHttpOperation.PostUpdateOrder, (ObjectDictionary)request.MergeIntoObjectDictionary());
            var res = await _restRequestService.SendRequestObjectAsync(request.AccountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
            result.AccountId = request.AccountId;
            result.Exchange = request.Exchange;
            result.Timestamp = result.Timestamp;
            return result;
        }

        public async Task<OrderDto[]> DeleteAllOrdersAsync(Xchange exchange, Guid accountId, string symbol, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.PostCancelAllOrder, new ObjectDictionary() { { IXchangeDescriptorService.SymbolString, symbol } });
            var res = await _restRequestService.SendRequestObjectAsync(accountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto[]>(res.Result);
            result.Each(p => { p.AccountId = accountId; p.Exchange = exchange; p.Timestamp = res.Timestamp; });
            return result;
        }

        public async Task<OrderDto> DeleteOrderAsync(Xchange exchange, Guid accountId, string symbol, string orderId, CancellationToken token = default)
        {
            var route = _descriptorService.RequestDictionary(exchange, XchangeHttpOperation.PostCancelOrder, new ObjectDictionary() { { IXchangeDescriptorService.OrderIdString, orderId }, { IXchangeDescriptorService.SymbolString, symbol } });
            var res = await _restRequestService.SendRequestObjectAsync(accountId, route, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
            result.AccountId = accountId;            
            result.Exchange = exchange;
            result.Timestamp = result.Timestamp;
            return result;
        }
    }
}