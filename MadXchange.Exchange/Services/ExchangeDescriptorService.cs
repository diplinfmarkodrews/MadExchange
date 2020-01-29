using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MadXchange.Exchange.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{
    public interface IExchangeDescriptorService 
    {
        public ExchangeDescriptor GetExchangeDescriptor(Exchanges exchange);
    }
    public class ExchangeDescriptorService : IExchangeDescriptorService
    {

        private readonly Dictionary<Exchanges, ExchangeDescriptor> _exchangeDescriptors;
        public ExchangeDescriptorService(Dictionary<Exchanges, ExchangeDescriptor> descriptorDict)
        {
            _exchangeDescriptors = descriptorDict;  
        }

        public ExchangeDescriptor GetExchangeDescriptor(Exchanges exchange) 
        {
            return _exchangeDescriptors[exchange];
        }
    }
}
