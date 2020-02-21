using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MadXchange.Exchange.Domain.Models
{
    /// <summary>
    /// Gets generated for each request and holds individual information to generate final request
    /// </summary>



    public class XchangeRequestObject //: IReturn<HttpResponseDto>
    {
        public string Url { get; }
        public string Method { get; }
        public SortedDictionary<string, object> RequestParameter { get; }
        public Xchange Exchange { get; }
        private KeyValuePair<string, string> _signature;
        private readonly string ApiKeyString;
        private readonly string TimeStampString;
        private readonly string SignString;
        private long _signStamp;
        private string _requestString;

        public XchangeRequestObject(XchangeDescriptor descriptor, EndPoint endPoint, ObjectDictionary paramDictionary = default)
        {
            Exchange = (Xchange)descriptor.Id;
            Url = endPoint.Url;
            Method = endPoint.Method;
            ApiKeyString = descriptor.ApiKeyString;
            TimeStampString = descriptor.TimeStampString;
            SignString = descriptor.SignString;
            RequestParameter = new SortedDictionary<string, object>();
            if (paramDictionary == default) return;
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
                if (paramName.Required) throw new InvalidOperationException($"Route parameter: {paramName.ExtName} can not be null, please verify exchange descritor:{Exchange.ToString()}");
                return;
            }
            RequestParameter.Add(paramName.ExtName, paramValue);
        }
        internal string GetSignedUrl() 
        { 
            var res = Url + "?" + BuildQueryString()+"&"+_signature.Key + "="+_signature.Value; 
            return res; 
        }
        internal string ToOrderedJson()
        {
            var res = RequestParameter.ToObjectDictionary();
            res.Add(_signature.Key, _signature.Value);
            _requestString = res.ToJson();
            return _requestString;
        }

        /// <summary>
        /// To add Apikey headers to the ObjectDictionary which carries our request parameters, it returns the querystring
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
            _signature = new KeyValuePair<string, string>(SignString, sign);
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