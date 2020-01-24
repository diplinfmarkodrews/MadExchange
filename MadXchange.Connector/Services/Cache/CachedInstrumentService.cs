using MadXchange.Common.Helpers;
using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using ServiceStack;
using MadXchange.Exchange.Domain.Cache;

namespace MadXchange.Connector.Services.Cache
{
    public class CachedInstrumentService : CacheStorage<InstrumentCacheObject>, ICachedInstrumentService
    {
        
        public CachedInstrumentService(IDistributedCache cache) : base("instrument", cache)
        {

        }
        public void UpdateInstrument(Exchanges exchange, string symbol, Instrument instrumentDto) 
        {
            if(instrumentDto.TimeStamp != default) 
            {

                var symbolStore = Get($"{exchange}{symbol}");
                //check if update is valid and needed
                if (!(symbolStore is null))
                {
                    if (instrumentDto.TimeStamp <= symbolStore.Instrument.TimeStamp)
                    {
                        return;
                    }
                }
                else //if no value has been stored before
                {
                    symbolStore = new InstrumentCacheObject() { Instrument = instrumentDto };
                    Set($"{exchange}{symbol}", symbolStore);
                    return;
                }
                symbolStore.Instrument.PopulateWithNonDefaultValues(instrumentDto);
                Set($"{exchange}{symbol}", symbolStore);
            }
        }
        public async Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol) 
        {
            
            var instrumentCacheObject = await GetAsync($"{exchange}{symbol}");
            if (instrumentCacheObject is null) throw new MethodAccessException($"error accessing instrument cache");
            return instrumentCacheObject.Instrument;


        }
        public async Task<IOrderBook> GetOrderBookAsync(Exchanges exchange, string symbol) 
        {
            throw new NotImplementedException();
        }

     
    }
}
