using MadXchange.Common.Mex.Interfaces;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Common.Mex.Specification.RestClientSpec
{
    public class RestClientOrder : RestClientExtend
    {

        public RestClientOrder(IConfiguration config, IUserAccount account, IExpiresTimeProvider timeProvider, ISignatureProvider signatureProvider) : base(config, account, signatureProvider, timeProvider) { }

        public async Task<IEnumerable<T>> GetOpenOrdersAsync<T>(CancellationToken token)
        {
            var req = new RestRequest(new Uri($"Orders/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[0] { });
            var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
            return rest.Data;
        }

        public async Task<IEnumerable<T>> GetOpenOrdersAsync<T>(string symbol, CancellationToken token)
        {
            var req = new RestRequest(new Uri($"Orders/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol) });
            var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
            return rest.Data;
        }

        public async Task<T> GetOrderByIDAsync<T>(string symbol, string orderID, CancellationToken token)
        {
            var req = new RestRequest(new Uri($"Orders/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[2] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:OrderIDParameterName").Value, orderID) });
            var rest = await ExecuteTaskAsync<T>(req);
            return rest.Data;
        }

        //public async Task<T> CreateOrder<T>(IOrderPostRequest request, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.POST);
        //    req = SignRequest(req, new KeyValuePair<string, string>[0] { });
        //    var rest = await ExecuteTaskAsync<T>(req);
        //    return rest..Data;
        //}

        //public async Task<IEnumerable<T>> CancelAllOrders(string symbol, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.POST);
        //    req = SignRequest(req, new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol) });
        //    var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
        //    return rest.Data;
        //}

        //public async Task<IEnumerable<T>> CancelAllOrders(CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.POST);
        //    req = SignRequest(req, new KeyValuePair<string, string>[0] { });
        //    var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
        //    return rest.Data;
        //}

        //public async Task<T> CancelOrder(string symbol, string orderID, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.POST);
        //    req = SignRequest(req, new KeyValuePair<string, string>[2] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:OrderIDParameterName").Value, orderID) });
        //    var rest = await ExecuteTaskAsync<T>(req);
        //    return rest.Data;
        //}

        //public async Task<T> ChangeOrder(IOrderPutRequest request, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.PUT);
        //    req = SignRequest(req, new KeyValuePair<string, string>[2] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, request.Symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:OrderIDParameterName").Value, request.OrderId) });
        //    var rest = await ExecuteTaskAsync<T>(req);
        //    return rest.Data;
        //}


        //public async Task<T> ClosePosition(string symbol, decimal? price, int amount, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.GET);
        //    if (price.HasValue)
        //    {
        //        req = SignRequest(req, new KeyValuePair<string, string>[3] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:PriceParameterName").Value, price.ToString()), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:AmountParameterName").Value, amount.ToString()) });
        //    }
        //    else 
        //    {
        //        req = SignRequest(req, new KeyValuePair<string, string>[2] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:AmountParameterName").Value, amount.ToString()) });
        //    }
        //    var rest = await ExecuteTaskAsync<T>(req);
        //    return rest.Data;
        //}

        //public async Task<T> ClosePosition(string symbol, OrderSide side, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.GET);
        //    req = SignRequest(req, new KeyValuePair<string, string>[2] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol), new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SideParameterName").Value, side.ToString()) });
        //    var rest = await ExecuteTaskAsync<T>(req);
        //    return rest.Data;
        //}

        //public async Task<IEnumerable<T>> QueryLastOrders(string symbol, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Orders/"), Method.GET);
        //    req = SignRequest(req, new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol) });
        //    var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
        //    return rest.Data;
        //}
    }
}
