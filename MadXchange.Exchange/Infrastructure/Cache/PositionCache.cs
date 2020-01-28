using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class PositionCache : CacheStorage<PositionCacheObject>
    {
        public PositionCache(IDistributedCache cache) : base("position", cache) { }
    }
}
