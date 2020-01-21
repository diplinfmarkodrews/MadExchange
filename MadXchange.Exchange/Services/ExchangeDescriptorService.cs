using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{
    public interface IExchangeDescriptorService 
    {
        public ExchangeDescriptor GetExchangeDescriptor(Exchange.Domain.Models.Exchanges exchange);
    }
    public class ExchangeDescriptorService
    {

        private readonly Dictionary<Exchange.Domain.Models.Exchanges, ExchangeDescriptor> _exchangeDescriptors;
        public ExchangeDescriptorService(Dictionary<Exchange.Domain.Models.Exchanges, ExchangeDescriptor> descriptorDict)
        {
            _exchangeDescriptors = descriptorDict;  
        }
        public ExchangeDescriptor GetExchangeDescriptor(Exchange.Domain.Models.Exchanges exchange) 
        {
            return _exchangeDescriptors[exchange];
        }
    }
}
