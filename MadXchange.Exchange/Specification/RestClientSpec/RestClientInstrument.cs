using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Common.Mex.Specification.RestClientSpec
{
    public class RestClientInstrument : RestClientExtend
    {

        public RestClientInstrument(IConfiguration config, IUserAccount account, IExpiresTimeProvider timeProvider, ISignatureProvider signatureProvider) : base(config, account, signatureProvider, timeProvider) 
        {
            
        }
        public async Task<T> GetInstrumentAsync<T>(string symbol, CancellationToken token)
        {
            var req = new RestRequest(new Uri(_configuration.GetSection("Route:Instrument").Value), Method.GET);
            req.AddParameter(_configuration.GetSection("Parameter:SymbolParameterName").Value, symbol);
            var rest = await ExecuteTaskAsync<T>(req);
            return rest.Data;
        }
    }
}
