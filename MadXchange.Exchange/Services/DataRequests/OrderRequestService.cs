using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using MadXchange.Exchange.Dto.Http;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IOrderRequestService 
    {
        Task<IEnumerable<Order>> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol);
        Task<Order> PostNewOrderAsync(OrderPostRequestDto request);
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
        public async Task<IEnumerable<Order>> GetOpenOrdersAsync(Exchanges exchange, Guid accountId, string symbol) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(exchange);
            var route = descriptor.RouteGetOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            if(symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], symbol);
            }                        
            var res = await _restRequestService.SendGetAsync<WebResponseDto>(accountId, url, parameter).ConfigureAwait(false);
            //Mapping
            var result = res.Result.ConvertTo<IEnumerable<Order>>();
            return result;

        }
        public async Task<Order> PostNewOrderAsync(OrderPostRequestDto request) 
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(request.Exchange);
            var route = descriptor.RoutePlaceOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;
            
            if (request.Symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            var res = await _restRequestService.SendPostAsync<WebResponseDto>(request.AccountId, url, parameter).ConfigureAwait(false);
            //Mapping
            var result = res.Result.ConvertTo<Order>();
            return result;
        }
        public async Task<Order> PostUpdateOrderAsync(OrderPostRequestDto request)
        {
            var descriptor = _descriptorService.GetExchangeDescriptor(request.Exchange);
            var route = descriptor.RoutePlaceOrder;
            var url = $"{descriptor.BaseUrl}/{route.Url}";
            var parameter = string.Empty;

            if (request.Symbol != string.Empty)
            {
                parameter.AddQueryParam(route.Parameter[0], request.Symbol);
            }
            var res = await _restRequestService.SendPostAsync<WebResponseDto>(request.AccountId, url, parameter).ConfigureAwait(false);
            //Mapping
            var result = res.Result.ConvertTo<Order>();
            return result;
        }
    }
}
