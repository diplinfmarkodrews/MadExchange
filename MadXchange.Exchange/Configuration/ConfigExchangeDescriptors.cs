using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using System.Runtime.Serialization;
using System.Reflection;
using MadXchange.Exchange.Contracts;

namespace MadXchange.Exchange.Configuration
{
    public static class ConfigExchangeDescriptors
    {
        
        
        private static IConfiguration _configuration;

        public static void SetConfig(IConfiguration config) => _configuration = config.GetSection("ExchangeDescriptors");
        //thats the main configuration function
        public static Dictionary<Exchanges, ExchangeDescriptor> ReadExchangeDescriptorConfiguration()
        {
                       
            var descriptors = _configuration.GetChildren();
            var exchangeDictionary = new Dictionary<Exchanges, ExchangeDescriptor>();                        
            if (descriptors.Count() == 0) return exchangeDictionary;

            foreach (var exchange in descriptors)
            {              
                var exchangeDescriptor = new ExchangeDescriptor();
                exchangeDescriptor.Name = exchange.GetValue<string>("Name");
                exchangeDescriptor.BaseUrl = exchange.GetValue<string>("BaseUrl");
                exchangeDescriptor.SocketUrl = exchange.GetValue<string>("SocketUrl");
                var exchangeEnum = Enum.Parse<Exchanges>(exchangeDescriptor.Name);
                exchangeDescriptor.Id = (int)exchangeEnum;   
                exchangeDescriptor.EndPoints.RegisterExchangeEndPoints("GET", exchange.GetSection("Routes:GET"));
                exchangeDescriptor.EndPoints.RegisterExchangeEndPoints("POST", exchange.GetSection("Routes:POST"));
                exchangeDescriptor.EndPoints.Values.Each(f => f.Url = exchangeDescriptor.BaseUrl.CombineWith(f.Url));
                exchangeDictionary.Add(exchangeEnum, exchangeDescriptor);                               
            }
            return exchangeDictionary;
        }

        private static Dictionary<string, EndPoint> RegisterExchangeEndPoints(this Dictionary<string, EndPoint> endPointDic, string routeType, IConfigurationSection routes)
        {

            if (endPointDic is null) endPointDic = new Dictionary<string, EndPoint>();
            foreach (var r in routes.GetChildren())
            {
                var endPoint = ReadEndPoint(r);
                endPointDic.Add($"{routeType}{r.Key}", endPoint);
            }
            return endPointDic;
        }

        private static EndPoint ReadEndPoint(IConfigurationSection cSection)
        {
            var endP = new EndPoint();
            endP.Url = cSection.GetSection("Url").Value;
            endP.Name = cSection.Key;
            var paramSection = cSection.GetSection("Parameter").GetChildren();
            var parameterCount = paramSection.Count();
            if (parameterCount > 0)
            {
                endP.Parameter = new Parameter[parameterCount];
                for (int i = 0; i < parameterCount; i++)
                {
                    var paramAt = paramSection.ElementAt(i);
                    endP.Parameter[i] = new Parameter();
                    endP.Parameter[i].IsRequired = bool.Parse(paramAt.GetSection("Required")?.Value);
                    var domainName = paramAt.GetSection("Domain")?.Value;
                    domainName = domainName == string.Empty ? paramAt.Key : domainName;
                    endP.Parameter[i].Param = new ValueTuple<string, string>(domainName, paramAt.Key);
                    endP.Parameter[i].Type = paramAt.GetSection("Type")?.Value;
                }
            }
            endP.Result = cSection.GetSection("Result")?.Value;
            return endP;
        }
    }
}
