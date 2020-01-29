using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public class OrderCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public Order Order { get; set; }
        public OrderCacheObject(Guid accountId) 
        {
            AccountId = accountId;
        }
        public bool IsValid() => 
               Order is null 
            || Order.Timestamp == default 
            || Order.Symbol == default 
            || Order.OrderId == default 
            ? false 
            : true;
    }
}
