using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class OrderCache : CacheStorage<OrderCacheObject>, IOrderCache
    {
        public OrderCache(IDistributedCache cache) : base("order", cache)
        {
        }

        /// <summary>
        /// Primary function to read orders from cache.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderAsync(Guid accountId, string symbol, string orderId)
        {
            var orderById = await GetAsync($"{accountId}{symbol}{orderId}");
            if (orderById is null) return null;
            return orderById.Order;
        }

        public void RemoveOrder(Guid accountId, string symbol, string orderId)
        {
            Remove($"{accountId}{symbol}{orderId}");
        }

        /// <summary>
        /// This adds an new order to the cache at the given adress. old values will be overwritten!
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        public void AddOrder(Guid accountId, string symbol, string orderId, Order order)
        {
            var orderCacheObj = new OrderCacheObject(accountId) { Order = order };
            Set($"{accountId}{symbol}{orderId}", orderCacheObj);
        }

        /// <summary>
        /// updates the order in cache after checking actual cache object. If none existent, it adds a new cacheObject, else checks wether it needs update by timestamp
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool UpdateOrder(Guid accountId, string symbol, string orderId, Order order)
        {
            var orderCacheObj = Get($"{accountId}{symbol}{orderId}");
            if (orderCacheObj is null)
            {
                orderCacheObj = new OrderCacheObject(accountId) { Order = order };
                Set($"{accountId}{symbol}{orderId}", orderCacheObj);
                return true;
            }
            if (order.Timestamp > orderCacheObj.Order.Timestamp)
            {
                Set($"{accountId}{symbol}{orderId}", orderCacheObj);
                return true;
            }
            return false;
        }
    }
}