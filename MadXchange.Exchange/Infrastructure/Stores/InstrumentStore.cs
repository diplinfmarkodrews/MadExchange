using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface IInstrumentStore 
    {
        
        InstrumentCacheObject GetInstrument(string key, Guid id, Xchange exchange, string symbol);
    }
    public class InstrumentStore : IInstrumentStore
    {
        private readonly Dictionary<string, InstrumentCacheObject> _instrumentCacheObjects = new Dictionary<string, InstrumentCacheObject>();
     
        public InstrumentCacheObject GetInstrument(string key, Guid id, Xchange exchange, string symbol)
            => _instrumentCacheObjects.GetValueOrDefault(key, new InstrumentCacheObject(id, exchange, symbol));
                    
    }
}
