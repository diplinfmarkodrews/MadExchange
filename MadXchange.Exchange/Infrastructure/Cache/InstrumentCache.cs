using MadXchange.Common.Helpers;
using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using ServiceStack;
using MadXchange.Exchange.Domain.Cache;

namespace MadXchange.Exchange.Infrastructure.Cache
{
  

    public class InstrumentCache : CacheStorage<InstrumentCacheObject>, IInstrumentCache
    {
        
        public InstrumentCache(IDistributedCache cache) : base("instrument", cache) { }
        /// <summary>
        /// check if DataObj is valid
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="instrumentDto"></param>
        /// <returns></returns>
        public long UpdateInstrument(Exchanges exchange, string symbol, Instrument instrumentDto) 
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

        public async Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol) 
        {            
            var instrumentCacheObject = await GetAsync($"{exchange}{symbol}");
            if (instrumentCacheObject is null) return null;
            return instrumentCacheObject.Instrument;
        }

        public async Task<OrderBook> GetOrderBookAsync(Exchanges exchange, string symbol) 
        {
            throw new NotImplementedException();
        }     
    }
}
