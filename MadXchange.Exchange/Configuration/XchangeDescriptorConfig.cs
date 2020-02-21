using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Helpers;
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
        private XchangeDescriptor[] _exchangeDescriptors;
        private static Dictionary<string, Type> _dataTypes;
        
        public XchangeDescriptorConfig(IConfiguration config)
        {
            _config = config.GetSection("ExchangeDescriptors");
            _dataTypes = XchangeConfigToolkit.GenerateTypeDictionary();
            _exchangeDescriptors = ReadExchangeDescriptorConfiguration(_config);
        }

        public XchangeDescriptor[] StoredExchangeDescriptorConfiguration => _exchangeDescriptors;

        public XchangeDescriptor[] ReadExchangeDescriptorConfiguration() => ReadExchangeDescriptorConfiguration(_config);
        //thats the main configuration function
        private XchangeDescriptor[] ReadExchangeDescriptorConfiguration(IConfiguration section)
        {
            var descriptors = section.GetChildren();
            int exchangeConfigs = descriptors.Count();
            XchangeDescriptor[] exchanges = new XchangeDescriptor[exchangeConfigs + 1];
            if (exchangeConfigs == 0)
                return exchanges;

            foreach (var exchange in descriptors)
            {
                var exchangeDescriptor = new XchangeDescriptor();                               
                exchangeDescriptor.Name = exchange.GetValue<string>("Name");
                exchangeDescriptor.BaseUrl = exchange.GetValue<string>("BaseUrl");  
                exchangeDescriptor.ApiKeyString = exchange.GetValue<string>("ApiKeyString");
                exchangeDescriptor.SignString = exchange.GetValue<string>("SignString");
                exchangeDescriptor.TimeStampString = exchange.GetValue<string>("TimeStampString");
                var exchangeEnum = Enum.Parse<Xchange>(exchangeDescriptor.Name);
                exchangeDescriptor.Id = (int)exchangeEnum;
                exchangeDescriptor.DomainTypes = XchangeConfigToolkit.GenerateDomainTypes(_dataTypes, exchange);                                
                exchangeDescriptor.EndPoints = GeneratingEndPoints(exchange, exchangeDescriptor.BaseUrl);
                exchangeDescriptor.SetEndPointReturnTypes();
                exchangeDescriptor.SocketDescriptor = ReadSocketConfig(exchange);
                if (VerifyExchangeDescriptor(exchangeDescriptor))
                    exchanges[exchangeDescriptor.Id] = exchangeDescriptor;
            }
            return exchanges;
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
        private static EndPoint[] GeneratingEndPoints(IConfigurationSection exchange, string baseUrl)
        {
            var endPoints = XchangeConfigToolkit.ReadExchangeEndPoints(null, exchange.GetSection("Routes"));
            var endPointArray = new EndPoint[typeof(XchangeHttpOperation).GetTypeInfo().Fields().Length + 1];
            foreach (var eP in endPoints)
            {
                                
                eP.Value.Url = $"{baseUrl}{eP.Value.Url}";
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

        

        
    }
}