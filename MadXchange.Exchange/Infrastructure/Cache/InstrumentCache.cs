using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    /// <summary>
    /// Defines implementation for accessing the distributed cache. ToDo Alternatively provide implementation for IRedisClient or typed IRedisTypedClient
    /// https://github.com/ServiceStack/ServiceStack.Redis
    /// </summary>
    [Serializable]
    public class InstrumentCache : CacheStorage<InstrumentCacheObject>, IInstrumentCache
    {
        public InstrumentCache(IRedisClientsManager cache) : base("instrument", cache)
        {
        }

        /// <summary>
        /// check if DataObj is valid
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="instrumentDto"></param>
        /// <returns></returns>
        public long UpdateInstrument(Xchange exchange, string symbol, Instrument instrumentDto)
        {
            var symbolStore = Get($"{exchange}{symbol}");
            //check if update is valid and needed (specific time)
            if (!(symbolStore is null))
            {
                if (instrumentDto.Timestamp <= symbolStore.Instrument.Timestamp)
                {
                    return symbolStore.Instrument.Timestamp.Ticks;
                }
            }
            else //if no value has been stored before
            {
                symbolStore = new InstrumentCacheObject() { Instrument = instrumentDto };
                Set($"{exchange}{symbol}", symbolStore);
                return symbolStore.Instrument.Timestamp.Ticks;
            }
            symbolStore.Instrument.PopulateWithNonDefaultValues(instrumentDto);
            Set($"{exchange}{symbol}", symbolStore);
            return symbolStore.Instrument.Timestamp.Ticks;
        }

        public async Task<Instrument> GetInstrumentAsync(Xchange exchange, string symbol)
        {
            var instrumentCacheObject = await GetAsync($"{exchange}{symbol}");
            if (instrumentCacheObject is null) return null;
            return instrumentCacheObject.Instrument;
        }

        public async Task<OrderBook> GetOrderBookAsync(Xchange exchange, string symbol)
        {
            throw new NotImplementedException();
        }
    }
}