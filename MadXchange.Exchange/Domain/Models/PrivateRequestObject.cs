using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Types;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Domain.Models
{
    /// <summary>
    /// Gets generated for each request and holds individual information to generate final request
    /// </summary>
    public class XchangeRequestObject : IReturn<WebResponseDto>
    {
        public string Url { get; }
        public string Method { get; }
        public Dictionary<string, object> RequestParameter { get; }
        public Xchange Exchange { get; }
        private readonly string ApiKeyString;
        private readonly string TimeStampString;
        private readonly string SignString;
        private long _signStamp;

        public XchangeRequestObject(Xchange exchange, EndPoint endPoint, ObjectDictionary paramDictionary)
        {
            Exchange = exchange;
            Url = endPoint.Url;
            ApiKeyString = endPoint.ApiKeyString;
            TimeStampString = endPoint.TimeStampString;
            SignString = endPoint.SignString;
            RequestParameter = new ObjectDictionary();
            foreach (var p in paramDictionary)
                Add(endPoint.Parameter[p.Key], paramDictionary[p.Key]);//p.Key is DomainName
        }

        /// <summary>
        /// Used to generate initial request parameter Dictionary
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        private void Add(Parameter paramName, object paramValue)
        {
            if (paramValue is null)
            {
                if (paramName.IsRequired) throw new InvalidOperationException($"Route parameter: {paramName.ExtName} can not be null, please verify exchange descritor:{Exchange.ToString()}");
                return;
            }
            RequestParameter.Add(paramName.ExtName, paramValue);
        }

        internal string ToOrderedJson() => RequestParameter.ToJson();

        /// <summary>
        /// To add Apikey headers, it returns the querystring
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="timeStamp"></param>
        internal string AddApiKeyHeaders(string apiKey, long timeStamp)
        {
            RequestParameter[ApiKeyString] = apiKey;
            RequestParameter[TimeStampString] = timeStamp;
            return BuildQueryString();
        }

        internal XchangeRequestObject AddSignature(string sign)
        {
            _signStamp = DateTime.UtcNow.Ticks;
            RequestParameter[SignString] = sign;
            return this;
        }

        /// <summary>
        ///  returns the query string of this request
        ///
        /// </summary>
        /// <returns></returns>
        internal string GetParamterString() => BuildQueryString();

        private string BuildQueryString()
        {
            string result = string.Empty;
            foreach (var p in RequestParameter)
                result = $"{result}&{p.Key}={p.Value}";
            return result.Trim('&');
        }
    }
}