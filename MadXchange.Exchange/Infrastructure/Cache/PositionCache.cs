using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public interface IPositionCache
    {
        void Insert(Guid id, long timestamp, Position position);        
        void Update(Guid id, long timeStamp, Position[] insert, Position[] update, Position[] delete);
        void UpdateLeverage(Guid id, long timeStamp, Position leverage);//leverage is hold in Position

        Task InsertAsync(Guid id, long timestamp, Position position);
        Task UpdateAsync(Guid id, long timeStamp, Position[] insert, Position[] update, Position[] delete);
        Task UpdateLeverageAsync(Guid id, long timeStamp, Position leverage);
      

    }

    public sealed class PositionCache : CacheStorageTransient<PositionCacheObject>, IPositionCache
    {

        private IPositionStore _positionStore = new PositionStore();

        public PositionCache(IRedisClientsManager cache) : base("position", cache) { }
        
        //public async Task<Position> GetPositionAsync(Guid accountId, string symbol)                  
        //    => (await GetAsync($"{accountId}")).Position[symbol];
              
        public void Insert(Guid accountId, long timestamp, Position position)
        {
            var positionCache = _positionStore.GetPosition(accountId);
            _positionStore.SetPositionCacheObject(positionCache);
            positionCache.Insert(timestamp, position);
            Set($"{accountId}", positionCache);
            
        }

        public async Task InsertAsync(Guid id, long timestamp, Position position)
        {
            var positionCache = _positionStore.GetPosition(id);
            _positionStore.SetPositionCacheObject(positionCache);
            positionCache.Insert(timestamp, position);
            await SetAsync($"{id}", positionCache);
        }

        public void Update(Guid id, long timeStamp, Position[] insert, Position[] update, Position[] delete)
        {
            var positionCache = _positionStore.GetPosition(id);
            positionCache.Update(timestamp: timeStamp,
                                    insert: insert,
                                    update: update,
                                    delete: delete);
            
            Set($"{id}", positionCache);
        }

        public async Task UpdateAsync(Guid id, long timeStamp, Position[] insert, Position[] update, Position[] delete)
        {
            var positionCache = _positionStore.GetPosition(id);
            positionCache.Update(timestamp: timeStamp,
                                     insert: insert,
                                     update: update,
                                     delete: delete);

            await SetAsync($"{id}", positionCache);
        }

        public void UpdateLeverage(Guid id, long timeStamp, Position leverage)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLeverageAsync(Guid id, long timeStamp, Position leverage)
        {
            throw new NotImplementedException();
        }
    }
}