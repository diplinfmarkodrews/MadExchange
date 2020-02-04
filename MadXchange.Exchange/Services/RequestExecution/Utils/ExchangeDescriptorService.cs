using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MadXchange.Connector;
using ServiceStack;
using MadXchange.Exchange.Types;
using System;
using Microsoft.Extensions.Logging;

namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IExchangeDescriptorService 
    {
       
        /// <summary>
        /// should be used to fetch predefined stringDictionaries, with first string as route to form request
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        EndPoint GetExchangeEndPoint(Exchanges exchange, string route);        

    }
    /// <summary>
    /// Todo: 
    ///     -efficient way to layout routes => stringDictionary
    ///     -Validation of descriptors, report on config, endpoints and types
    ///     -Update descriptors on config update
    /// </summary>
    public class ExchangeDescriptorService : IExchangeDescriptorService
    {
        
        private readonly ILogger _logger;     
        private readonly Dictionary<Exchanges, ExchangeDescriptor> _exchangeDescriptors;

        public ExchangeDescriptorService(ILogger<ExchangeDescriptorService> logger) 
        {
            _logger = logger;
            _exchangeDescriptors = ConfigExchangeDescriptors.ReadExchangeDescriptorConfiguration();
            _logger.LogInformation($"Exchangedescriptors loaded", _exchangeDescriptors);
        }
        ///can both be put together, providing a string dictionary as function layout
        //public string ReturnExchangeRoute(Exchanges exchange, string route) => $"{GetExchangeDescriptor(exchange).BaseUrl}{GetExchangeEndPoint(exchange, route).Url}";
        public EndPoint GetExchangeEndPoint(Exchanges exchange, string route) => _exchangeDescriptors[exchange].EndPoints[route];
        
        
        
        private ExchangeDescriptor GetExchangeDescriptor(Exchanges exchange) => _exchangeDescriptors[exchange];

        //register data contracts for de-/serialization of types 

       







    }
}
