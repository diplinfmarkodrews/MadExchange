using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public interface IMarginCache
    {
        void Set(Guid accountId, string symbol, Margin item);
        long Update(Guid accountId, string symbol, Margin item);
        Task<Margin> GetAsync(Guid accountId, string symbol);
        Task RemoveAsync(Guid accountId, string symbol);
    }
    public class MarginCache : CacheStorageTransient<MarginCacheObject>, IMarginCache
    {
        public MarginCache(IRedisClientsManager cache) : base("margin", cache)
        {
        }

        public void Set(Guid accountId, string symbol, Margin item)
        {
            var cacheObj = new MarginCacheObject(accountId)
            {
                MarginObj = item
            };
            Set($"{accountId}{symbol}", cacheObj);
        }

        public long Update(Guid accountId, string symbol, Margin item)
        {
            var marginObj = Get($"{accountId}{symbol}");
            if (marginObj is null)
            {
                marginObj = new MarginCacheObject(accountId)
                {
                    MarginObj = item
                };
                Set($"{accountId}{symbol}", marginObj);
            }
            else
            {
                if (marginObj.MarginObj.Timestamp < item.Timestamp)
                {
                    marginObj.MarginObj.PopulateWithNonDefaultValues(item);
                    Set($"{accountId}{symbol}", marginObj);
                }
            }
            return marginObj.MarginObj.Timestamp.Ticks;
        }

        public async Task<Margin> GetAsync(Guid accountId, string symbol)
        {
            var marginStore = await GetAsync($"{accountId}{symbol}");
            return marginStore.MarginObj;
        }

        public async Task RemoveAsync(Guid accountId, string symbol)
        {
            await RemoveAsync($"{accountId}{symbol}");
        }
    }
}