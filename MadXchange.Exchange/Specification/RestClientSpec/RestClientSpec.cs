
using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;

namespace MadXchange.Common.Mex.Specification
{



    public class RestClientExtend : RestClient
    {


        protected readonly ISignatureProvider _signatureProvider;
        protected readonly IExpiresTimeProvider _timeProvider;
        protected readonly IConfiguration _configuration;
        protected readonly string _key;
        protected readonly string _secret;
        protected readonly RestSharp.IRestClient _client;
        
        public RestClientExtend(IConfiguration config, IUserAccount user, ISignatureProvider signatureProvider, IExpiresTimeProvider timeProvider) : base()
        {
            _configuration = config;
            BaseUrl = user.Environment == Env.Test ? new Uri(config.GetSection("ConnectionString:Rest:Test").Value) : new Uri(config.GetSection("ConnectionString:Rest:Real").Value);
            _key = user.ApiKey;
            _secret = user.ApiSecret;
            _signatureProvider = signatureProvider;
            _timeProvider = timeProvider;
           
        }
        
        public virtual RestRequest SignRequest(RestRequest request, params KeyValuePair<string,string>[] para)  
        {
            //Http                                  
            request.AddParameter(_configuration.GetSection("key-format").Value, _key);
            request.AddParameter(_configuration.GetSection("timestamp").Value, _timeProvider.Get());
            foreach (var p in para) 
            {
                request.AddParameter(p.Key, p.Value);
            }
            string signString = string.Empty;
            request.Parameters.ForEach(p =>
            {
                signString += $"{p.Name}={p.Value},";

            });
            signString.Remove(signString.Length - 2);
            var sign = _signatureProvider.CreateSignature(_secret, signString);
            request.AddParameter(_configuration.GetSection("key-format").Value, sign);
            return request;
        }                 
    }
}
