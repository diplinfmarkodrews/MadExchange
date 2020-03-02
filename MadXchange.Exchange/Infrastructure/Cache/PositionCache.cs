using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public interface IPositionCache
    {
        void SetPosition(Guid accountId, string symbol, Position position);
        Task<Position> GetPositionAsync(Guid accountId, string symbol);
    }

    public sealed class PositionCache : CacheStorageTransient<PositionCacheObject>, IPositionCache
    {
        public PositionCache(IRedisClientsManager cache) : base("position", cache)
        {
            
        }

        public async Task<Position> GetPositionAsync(Guid accountId, string symbol)                  
            => (await GetAsync($"{accountId}{symbol}"))?.Position;
              

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