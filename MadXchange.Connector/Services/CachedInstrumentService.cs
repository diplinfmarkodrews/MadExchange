using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public class CachedInstrumentService : ICachedPriceService
    {
        private readonly IDistributedCache _distributedCache;
        public CachedInstrumentService(IDistributedCache cache) 
        {
            _distributedCache = cache;
        }
        public async Task<IInstrument> GetPriceAsync(Guid ExchangeId, string symbol, OrderSide side) 
        {
            string price = side == OrderSide.Buy ? "ask" : "bid";
            var instrumentString = await _distributedCache.GetStringAsync($"{ExchangeId}_{symbol}_{price}");
            return null; decimal.Parse(instrumentString);

        }
        public async Task<IOrderBook> GetOrderBookAsync(Guid ExchangeId, string symbol) 
        {
            return null;
        }
    }
}
