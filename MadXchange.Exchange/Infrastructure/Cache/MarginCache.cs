using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class MarginCache : CacheStorage<MarginCacheObject>
    {
        public MarginCache(string baseAdress, IDistributedCache cache) : base(baseAdress, cache) { }
        
    }
}
