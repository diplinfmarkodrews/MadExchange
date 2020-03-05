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
            Orders.Add(order.OrderId, order);
            Timestamp = timestamp;

        }

        public void Update(long timestamp, Order[] insert, Order[] update, Order[] delete) 
        {
            insert.Each(order => Orders.Add(order.OrderId, order));
            update.Each(order => Orders[order.OrderId].PopulateWithNonDefaultValues(order));
            delete.Each(order => Orders.Remove(order.OrderId));
            Timestamp = timestamp;
        }
        public void InsertOrder(long timestamp, Order order)
            => Orders.Add(order.OrderId, order);

        public void UpdateOrder(Order order)        
            => Orders[order.OrderId].PopulateWithNonDefaultValues(order);

        private void Delete(Order order)
            => Orders.Remove(order.OrderId);

        public void ChangeOrderId(string oldOrderId, string newOrderId)
            => Orders.MoveKey(oldOrderId, newOrderId);

        public bool IsValid()                  
            => true;
    }
}