using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using System.Collections.Generic;
using ServiceStack;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IExchangeDescriptorService 
    {
        public ExchangeDescriptor GetExchangeDescriptor(Exchanges exchange);

    }
    /// <summary>
    /// Todo: 
    ///     -Validation of descriptors
    ///     -Update descriptors on config update
    /// </summary>
    public class ExchangeDescriptorService : IExchangeDescriptorService
    {

        private readonly Dictionary<Exchanges, ExchangeDescriptor> _exchangeDescriptors;
        public ExchangeDescriptorService(Dictionary<Exchanges, ExchangeDescriptor> descriptorDict)
        {
            _exchangeDescriptors = descriptorDict;
            RegisterDataContractAttributes();
        }

        private void RegisterDataContractAttributes() 
        {
            
            foreach (var exc in _exchangeDescriptors.Values) 
            {
                
            }
        }
        public ExchangeDescriptor GetExchangeDescriptor(Exchanges exchange) 
        {
            return _exchangeDescriptors[exchange];
        }
    }
}
