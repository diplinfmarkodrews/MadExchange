using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    public class PositionCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public Position Position { get; set; }
        public bool IsValid() => 
               Position is null 
            || Position.Timestamp == default 
            || Position.Symbol == default 
            ? false 
            : true;

    }
}
