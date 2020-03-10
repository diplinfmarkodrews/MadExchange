using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Types;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{

    public interface IInstrumentCache
    {
        public Task<Instrument> GetInstrumentAsync(Guid id, Xchange exchange, string symbol);
        public long Update(Guid id, Xchange exchange, string symbol, long timeStamp, Instrument item);       
    }

    /// <summary>
    /// 
    /// https://github.com/ServiceStack/ServiceStack.Redis
    /// </summary>

    public class InstrumentCache : CacheStorageTransient<InstrumentCacheObject>, IInstrumentCache
    {
        private readonly IInstrumentStore _instrumentStore = new InstrumentStore();

        public InstrumentCache(IRedisClientsManager cacheClientManager) : base("Instrument", cacheClientManager) { }
       

        /// <summary>
        /// Updates Instrument CacheInstances
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="instrumentDto"></param>
        /// <returns></returns>
        public long Update(Guid id, Xchange exchange, string symbol, long timeStamp, Instrument instrumentDto)
        {
            var keyString = $"{exchange}{symbol}{id}";
            var cacheObject = _instrumentStore.GetInstrument(keyString, id, exchange, symbol);            
            cacheObject.Update(timeStamp, instrumentDto);           
            Set(keyString, cacheObject);
            return cacheObject.Timestamp;
        }

        public async Task<Instrument> GetInstrumentAsync(Guid id, Xchange exchange, string symbol)        
            => (await GetAsync($"{id}{exchange}{symbol}")).Instrument;
                                

       

      
    }
}