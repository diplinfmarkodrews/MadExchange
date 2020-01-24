using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class OrderCache : CacheStorage<OrderCacheObject>
    {
        public OrderCache(string baseAdress, IDistributedCache cache) : base(baseAdress, cache) { }
    }
}
