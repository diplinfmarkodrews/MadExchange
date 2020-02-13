using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace MadXchange.Exchange.Configuration
{
    /// <summary>
    /// Todo Verification function
    /// </summary>
    public interface IXchangeDescriptorConfiguration
    {
        XchangeDescriptor[] ReadExchangeDescriptorConfiguration();

        XchangeDescriptor[] StoredExchangeDescriptorConfiguration { get; }
    }

    public class XchangeDescriptorConfig : IXchangeDescriptorConfiguration
    {
        private readonly IConfiguration _config;
        private readonly XchangeDescriptor[] _exchangeDescriptors;

        public XchangeDescriptorConfig(IConfiguration config)
        {
            _config = config.GetSection("ExchangeDescriptors");
            _exchangeDescriptors = ReadExchangeDescriptorConfiguration();
        }

        public XchangeDescriptor[] StoredExchangeDescriptorConfiguration => _exchangeDescriptors;

        //thats the main configuration function
        public XchangeDescriptor[] ReadExchangeDescriptorConfiguration()
        {
            var descriptors = _config.GetChildren();
            int exchangeConfigs = descriptors.Count();
            XchangeDescriptor[] exchanges = new XchangeDescriptor[exchangeConfigs + 1];
            if (exchangeConfigs == 0)
                return exchanges;

            foreach (var exchange in descriptors)
            {
                var exchangeDescriptor = new XchangeDescriptor();
                exchangeDescriptor.Name = exchange.GetValue<string>("Name");
                exchangeDescriptor.BaseUrl = exchange.GetValue<string>("BaseUrl");                
                var exchangeEnum = Enum.Parse<Xchange>(exchangeDescriptor.Name);
                exchangeDescriptor.Id = (int)exchangeEnum;
                exchangeDescriptor.ReadDomainTypes(exchange.GetSection("DomainTypes"));
                var endPoints = ReadExchangeEndPoints(exchange.GetSection("Routes"));
                exchangeDescriptor.EndPoints = GeneratingEndPoints(endPoints);
                exchangeDescriptor.EndPoints.Each(f => f.Url = exchangeDescriptor.BaseUrl.CombineWith(f.Url));
                exchangeDescriptor.SocketDescriptor = ReadSocketConfig(exchange);
                if (VerifyExchangeDescriptor(exchangeDescriptor))
                    exchanges[exchangeDescriptor.Id] = exchangeDescriptor;
            }
            return exchanges;
        }
        
        private ObjectDictionary ReadTypes(IConfigurationSection section) 
        {
            var result = new ObjectDictionary();
            var typesConfig = section.GetSection("Types").GetChildren();
            foreach (var type in typesConfig)
            {
                var members = type.GetChildren();
                foreach (var m in members)
                    result.Add(m.Key, m.Value);
            }
            return result;
        }
        private XchangeSocketDescriptor ReadSocketConfig(IConfigurationSection exchangeConfig)
        {
            var socketConfig = exchangeConfig.GetSection("Socket");
            var result = new XchangeSocketDescriptor()
            {
                SocketUrl = exchangeConfig.GetSection("SocketUrl").Value,
                KeepAliveInterval = exchangeConfig.GetSection("KeepAlive").Get<int>(),

            };

            
            var stringValues = socketConfig.GetChildren().Where(c => c.GetChildren().Count() == 1);
            result.CombinedStrings = new Dictionary<string, string[]>();

            return result;
        }

       

        private bool VerifyExchangeDescriptor(XchangeDescriptor descriptor)
        {
            if (descriptor.EndPoints[(int)XchangeHttpOperation.Unknown] != null) return false;
            return true;
        }

        /// <summary>
        /// returns array of valid endpoints
        /// </summary>
        /// <param name="endPoints"></param>
        /// <returns></returns>
        private EndPoint[] GeneratingEndPoints(Dictionary<string, EndPoint> endPoints)
        {
            var endPointArray = new EndPoint[typeof(XchangeHttpOperation).GetTypeInfo().Fields().Length];
            foreach (var eP in endPoints)
            {
                var operation = XchangeHttpOperation.Unknown;
                if (Enum.TryParse<XchangeHttpOperation>(eP.Key, out operation))
                {
                    endPointArray[(int)operation] = eP.Value;
                    continue;
                }
                endPointArray[(int)operation] = eP.Value;
            }
            return endPointArray;
        }

        private Dictionary<string, EndPoint> ReadExchangeEndPoints(IConfigurationSection routes)
        {
            var endPointDic = new Dictionary<string, EndPoint>();
            foreach (var r in routes.GetChildren())
            {
                var endPoint = ReadEndPoint(r);
                endPointDic.Add($"{r.Key}", endPoint);
            }
            return endPointDic;
        }

        private EndPoint ReadEndPoint(IConfigurationSection cSection)
        {
            var endP = new EndPoint();
            endP.Url = cSection.GetSection("Url")?.Value;
            endP.Name = cSection.Key;
            var paramSection = cSection.GetSection("Parameter").GetChildren();
            var parameterCount = paramSection.Count();
            if (parameterCount > 0)
            {
                endP.Parameter = new Dictionary<string, Parameter>();
                for (int i = 0; i < parameterCount; i++)
                {
                    var paramAt = paramSection.ElementAt(i);
                    var parameter = new Parameter();
                    paramAt.Bind(parameter);
                    endP.Parameter.Add(paramAt.Key, parameter);
                }
            }
            endP.Result = cSection.GetSection("Result")?.Value ?? cSection.Key;
            return endP;
        }

        private static IEnumerable<Type> ScanAssemblyForDataContracts()
        {
            var result = new List<Type>();
            Assembly assembly = typeof(InstrumentDto).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.HasAttribute<DataContractAttribute>())
                {
                    result.Add(type);
                }
            }
            return result;
        }
    }
}