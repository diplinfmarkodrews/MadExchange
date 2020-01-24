using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    public class PositionCacheObject 
    {
        public Guid AccountId { get; }
        public Position Position { get; set; } 
    }
}
