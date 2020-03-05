using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public interface IOrderCache
    {
      
        public Task<Dictionary<string, Order>> GetOrderAsync(Guid accountId);
        void InsertOrder(Guid id, long timestamp, Order insert);
        void Update(Guid id, long timeStamp, Order[] insert, Order[] update, Order[] delete);
    }

    public class OrderCache : CacheStorageTransient<OrderCacheObject>, IOrderCache
    {

        private IOrderStore _orderStore = new OrderStore();

        public OrderCache(IRedisClientsManager cache) : base("order", cache) { }
        
        /// <summary>
        /// Primary function to read orders from cache.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Order>> GetOrderAsync(Guid accountId)
        {
            var orderById = await GetAsync($"{accountId}");
            if (orderById is null) return null;

            return orderById.Orders;
        }

        /// <summary>
        /// updates the orderId in cache after checking actual cache object. 
        /// If none existent, it adds a new cacheObject, 
        /// else checks wether it needs update by timestamp
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public void UpdateOrderId(Guid accountId, string oldOrderId, string newOrderId, Order order)
        {
            var orderCacheObj = _orderStore.GetOrderCache(accountId);
            orderCacheObj.ChangeOrderId(oldOrderId, newOrderId);
            orderCacheObj.UpdateOrder(order);
            Set($"{accountId}", orderCacheObj);                                        
        }

        public void InsertOrder(Guid id, long timestamp, Order insert)
        {
            var orderCache = _orderStore.GetOrderCache(id);
            if (orderCache == null)
                _orderStore.Insert(id);

            orderCache.InsertOrder(timestamp, insert);
            Set($"{id}", orderCache);

        }

        public void Update(Guid id, long timeStamp, Order[] insert, Order[] update, Order[] delete)
        {
            var positionCache = _orderStore.GetOrderCache(id);
            positionCache?.Update(timestamp: timeStamp,
                                     insert: insert,
                                     update: update,
                                     delete: delete);

            Set($"{id}", positionCache);
        }
    }
}