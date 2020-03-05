using MadXchange.Exchange.Domain.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface IPositionStore 
    {
        PositionCacheObject GetPosition(Guid accountId);
        void SetPositionCacheObject(PositionCacheObject cacheObject);
    }

    public class PositionStore : IPositionStore
    {
        private Dictionary<Guid, PositionCacheObject> _positionCacheObjects = new Dictionary<Guid, PositionCacheObject>();

        public PositionCacheObject GetPosition(Guid accountId)
            => _positionCacheObjects.GetValueOrDefault(accountId, new PositionCacheObject(accountId));

        public void SetPositionCacheObject(PositionCacheObject cacheObject)
            => _positionCacheObjects[cacheObject.AccountId] = cacheObject;
    }
}
