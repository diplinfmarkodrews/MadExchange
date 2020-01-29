using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class PositionCache : CacheStorage<PositionCacheObject>, IPositionCache
    {
        public PositionCache(IDistributedCache cache) : base("position", cache) { }

        public async Task<Position> GetPositionAsync(Guid accountId, string symbol)
        {
            var positionCacheObj = await GetAsync($"{accountId}{symbol}");
            return positionCacheObj.Position;
        }

        public void SetPosition(Guid accountId, string symbol, Position position)
        {
            var positionCacheObj = new PositionCacheObject(accountId)
            {
                Position = position
            };
            Set($"{accountId}{symbol}", positionCacheObj);
        }
    }
}
