using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{

    public interface IInstrumentCache
    {
        public Task<Instrument> GetInstrumentAsync(Guid id, Xchange exchange, string symbol);

        public Task<OrderBook> GetOrderBookAsync(Guid id, Xchange exchange, string symbol);

        public long UpdateInstrument(Guid id, Xchange exchange, string symbol, long timeStamp, Instrument item);
    }

    /// <summary>
    /// 
    /// https://github.com/ServiceStack/ServiceStack.Redis
    /// </summary>

    public class InstrumentCache : CacheStorage<InstrumentCacheObject>, IInstrumentCache
    {
        private readonly IInstrumentStore _instrumentStore;
                
        public InstrumentCache(IInstrumentStore instrumentStore, IRedisClientsManager cacheClientManager) : base("Instrument", cacheClientManager)
        {
            _instrumentStore = instrumentStore;
           
        }

        /// <summary>
        /// Updates Instrument CacheInstances
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="instrumentDto"></param>
        /// <returns></returns>
        public long UpdateInstrument(Guid id, Xchange exchange, string symbol, long timeStamp, Instrument instrumentDto)
        {
            var keyString = $"{id}{exchange}{symbol}";
            var cacheObject = _instrumentStore.GetInstrument(keyString, id, exchange, symbol);            
            cacheObject.Update(timeStamp, instrumentDto);           
            Set(keyString, cacheObject);
            return cacheObject.Timestamp;
        }

        public async Task<Instrument> GetInstrumentAsync(Guid id, Xchange exchange, string symbol)        
            => (await GetAsync($"{id}{exchange}{symbol}")).Instrument;
                                

        public async Task<OrderBook> GetOrderBookAsync(Guid id, Xchange exchange, string symbol)
        {
            throw new NotImplementedException();
        }
    }
}