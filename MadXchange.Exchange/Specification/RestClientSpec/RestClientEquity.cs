using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Common.Mex.Specification.RestClientSpec
{
    public class RestClientEquity : RestClientExtend
    {

        public RestClientEquity(IConfiguration config, IUserAccount user, IExpiresTimeProvider timeProvider, ISignatureProvider signatureProvider) : base(config, user, signatureProvider, timeProvider) 
        {
            
           
        
        }

        public async Task<IEnumerable<T>> GetWalletHistoryAsync<T>(CancellationToken token)
        {
            RestRequest req = new RestRequest(new Uri("Wallet/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[0]);
            var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
            return rest.Data;
        }
        /// <summary>
        /// requests all available walletbalances of a client
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetMarginAsync<T>(CancellationToken token)
        {
            var req = new RestRequest(new Uri("Margin/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[0]);
            var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
            return rest.Data;
        }

        public async Task<T> GetMarginAsync<T>(string currency, CancellationToken token)
        {
            var req = new RestRequest(new Uri($"Margin/{currency}"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>() });
            var rest = await ExecuteTaskAsync<T>(req);
            return rest.Data;
        }

    }
}
