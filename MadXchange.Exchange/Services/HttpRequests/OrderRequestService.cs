using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
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
    public interface IOrderRequestService 
    {
        //Task<IQueryData<Order>> GetOrders(Exchanges exchange, Guid accountId, string symbol);
        Task<IEnumerable<Order>> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token);
        Task<Order> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token);
        Task<Order> PostUpdateOrderAsync(OrderPostRequestDto request, CancellationToken token);
        Task<Order> DeleteOrderAsync(Exchanges exchange, Guid accountId, string symbol, string orderId, CancellationToken token);
        Task<IEnumerable<Order>> DeleteAllOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token);
    }

    public class OrderRequestService : IOrderRequestService
    {

        private ILogger _logger;
        private IExchangeDescriptorService _descriptorService;
        private IRestRequestService _restRequestService;
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

        public async Task<IEnumerable<Order>> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token = default) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteGetOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if(symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }                        
            var res = await _restRequestService.SendGetAsync(accountId, url, parameter, token).ConfigureAwait(false);                        
            return res.result.FromJson<IEnumerable<Order>>();
        }
        /// <summary>
        /// Todo Parameter setting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Order> PostNewOrderAsync(OrderPostRequestDto request, CancellationToken token = default) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(request.Exchange);
            var route = descriptor.RoutePlaceOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            
            if (request.Symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            if (request.Quantity.HasValue) 
            {
                parameter.AddQueryParam(route.Parameter[1], $"{request.Quantity}");
            }
            var res = await _restRequestService.SendPostAsync(request.AccountId, url, parameter, token).ConfigureAwait(false);            
            return res.result.FromJson<Order>();
        }

        /// <summary>
        /// Todo parameter setting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Order> PostUpdateOrderAsync(OrderPostRequestDto request, CancellationToken token = default)
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(request.Exchange);
            var route = descriptor.RouteUpdateOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if (request.Symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            var res = await _restRequestService.SendPostAsync(request.AccountId, url, parameter, token).ConfigureAwait(false);                        
            return res.result.FromJson<Order>();
        }

        public async Task<IEnumerable<Order>> DeleteAllOrdersAsync(Exchanges exchange, Guid accountId, string symbol, CancellationToken token = default)
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteDeleteAllOrders;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            var res = await _restRequestService.SendPostAsync(accountId, url, parameter, token).ConfigureAwait(false);
            return res.result.FromJson<IEnumerable<Order>>();
        }

        public async Task<Order> DeleteOrderAsync(Exchanges exchange, Guid accountId, string symbol, string orderId, CancellationToken token = default)
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteDeleteOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if (symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }
            if (orderId != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[1], orderId);
            }
            var res = await _restRequestService.SendPostAsync(accountId, url, parameter, token).ConfigureAwait(false);
            return res.result.FromJson<Order>();
        }
       
    }
}
