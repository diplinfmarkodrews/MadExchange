using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using ServiceStack;

namespace MadXchange.Exchange.Domain.Cache
{
    [Serializable]
    public class OrderCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public Dictionary<string, Order> Orders { get; } = new Dictionary<string, Order>();
        public long Timestamp { get; private set; }

        public OrderCacheObject(Guid accountId)
        {
            AccountId = accountId;
        }

        public void Insert(long timestamp, Order order)
        {
            if (!Orders.TryAdd(order.OrderId, order))
                Orders[order.OrderId].PopulateWithNonDefaultValues(order);
            Timestamp = timestamp;
        }

        public void Update(long timestamp, Order[] insert, Order[] update, Order[] delete)
        {
            insert.Each(order =>
            {
                if (!Orders.TryAdd(order.OrderId, order))
                    Orders[order.OrderId].PopulateWithNonDefaultValues(order);
            });
                
            update.Each(order => Orders[order.OrderId].PopulateWithNonDefaultValues(order));
            delete.Each(order => Orders.TryRemove(order.OrderId, out order));
            Timestamp = timestamp;
        }
        public void InsertOrder(long timestamp, Order order)
            => Orders.TryAdd(order.OrderId, order);

        public void UpdateOrder(Order order)        
            => Orders[order.OrderId].PopulateWithNonDefaultValues(order);

        private void Delete(Order order)
            => Orders.TryRemove(order.OrderId, out order);

        public void ChangeOrderId(string oldOrderId, string newOrderId)
            => Orders.MoveKey(oldOrderId, newOrderId);

        public bool IsValid()                  
            => true;
    }
}