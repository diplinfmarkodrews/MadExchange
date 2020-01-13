using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Common.Mex.Specification.RestClientSpec
{
    public class RestClientIdentity : RestClientExtend
    {

        public RestClientIdentity(IConfiguration config, IUserAccount account, IExpiresTimeProvider timeProvider, ISignatureProvider signatureProvider) : base(config, account, signatureProvider, timeProvider) { }
        public async Task<IEnumerable<T>> GetApiKeyAsync<T>(CancellationToken token)
        {
            var req = new RestRequest(new Uri($"ApiKey/"), Method.GET);
            req = SignRequest(req, new KeyValuePair<string, string>[0] { });
            var rest = await ExecuteTaskAsync<IEnumerable<T>>(req);
            return rest.Data;
        }
    }
}
