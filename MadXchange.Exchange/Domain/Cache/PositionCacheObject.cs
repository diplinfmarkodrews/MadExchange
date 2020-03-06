using MadXchange.Exchange.Domain.Models;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Domain.Cache
{
    [Serializable]
    public class PositionCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public Dictionary<string, Position> Data { get; } = new Dictionary<string, Position>();
        public long Timestamp { get; private set; }

        public PositionCacheObject(Guid accountId)
        {
            AccountId = accountId;
        }

        public bool IsValid()
            => true;

        public void Insert(long timestamp, Position position)
        {
            Data[position.Symbol] = position;
            Timestamp = timestamp;
        }
        public void Update(long timestamp, Position[] insert, Position[] update, Position[] delete)
        {
            insert.Each(item => 
            {
                if (!Data.TryAdd(item.Symbol, item))
                    Data[item.Symbol].PopulateWithNonDefaultValues(item);
            });
            update.Each(item => Data[item.Symbol].PopulateWithNonDefaultValues(item));
            delete.Each(item => Data.TryRemove(item.Symbol, out item));
            Timestamp = timestamp;
        }
    }
}