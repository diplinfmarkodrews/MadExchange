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
    public interface IOrderRequestService 
    {
        
        Task<OrderDto[]> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token);
        Task<OrderDto> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token);
        Task<OrderDto> PostUpdateOrderAsync(OrderPostRequestDto request, CancellationToken token);
        Task<OrderDto> DeleteOrderAsync(Exchanges exchange, Guid accountId, string symbol, string orderId, CancellationToken token);
        Task<OrderDto[]> DeleteAllOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token);
    }

    public class OrderRequestService : IOrderRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
        private const string _domainString = "Order";
        public OrderRequestService(IExchangeDescriptorService exchangeDescriptorService, IRestRequestService restRequestService,  ILogger<OrderRequestService> logger) 
        {
            _descriptorService = exchangeDescriptorService;
            _restRequestService = restRequestService;
            _logger = logger;
        }

        //public async Task<IQueryData<Order>> GetOrders(Exchanges exchange, Guid accountId, string symbol, DateTime )
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<OrderDto[]> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token = default) 
        {
            var route = _descriptorService.GetExchangeEndPoint(exchange, symbol);            
            var parameter = string.Empty;
            if(symbol != string.Empty)
            {
                parameter = parameter.AddQueryParam(route.Parameter[0], symbol);
            }                        
            var res = await _restRequestService.SendGetAsync(accountId, route.Url, parameter, token).ConfigureAwait(false);
            var result = TypeSerializer.DeserializeFromString<OrderDto[]>(res.Result);
            return result;
        }
        /// <summary>
        /// Todo Parameter setting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<OrderDto> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token = default) 
        {
            var route = _descriptorService.GetExchangeEndPoint(request.Exchange, request.Symbol);
            var parameter = string.Empty;           
            if (request.Symbol != string.Empty)
            {
                parameter = parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            if (request.Quantity.HasValue) 
            {
                parameter = parameter.AddQueryParam(route.Parameter[1], $"{request.Quantity}");
            }
            if (request.Price.HasValue)
            {
                parameter = parameter.AddQueryParam(route.Parameter[2], $"{request.Price}");
            }
            var res = await _restRequestService.SendPostAsync(request.AccountId, route.Url, parameter, token).ConfigureAwait(false);                 
            return TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
        }

        public async Task<OrderDto> PostUpdateOrderAsync(OrderPostRequestDto request, CancellationToken token = default)
        {

            var route = _descriptorService.GetExchangeEndPoint(request.Exchange, _domainString);            
            var parameter = string.Empty;
            if (request.Symbol != string.Empty)
            {
                parameter = parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            var res = await _restRequestService.SendPostAsync(request.AccountId, route.Url, parameter, token).ConfigureAwait(false);                        
            return TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
        }

        public async Task<OrderDto[]> DeleteAllOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token = default)
        {

            var route = _descriptorService.GetExchangeEndPoint(exchange, symbol);            
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            var res = await _restRequestService.SendPostAsync(accountId, route.Url, parameter, token).ConfigureAwait(false);
            return TypeSerializer.DeserializeFromString<OrderDto[]>(res.Result);
        }

        public async Task<OrderDto> DeleteOrderAsync(Exchanges exchange, Guid accountId, string symbol, string orderId, CancellationToken token = default)
        {
            var route = _descriptorService.GetExchangeEndPoint(exchange, ""+_domainString);
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            if (orderId != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[1], orderId);
            }
            var res = await _restRequestService.SendPostAsync(accountId, route.Url, parameter, token).ConfigureAwait(false);
            return TypeSerializer.DeserializeFromString<OrderDto>(res.Result);
        }
       
    }
}
