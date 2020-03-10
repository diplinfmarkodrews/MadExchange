using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    
    public interface IOrderStore 
    {        
        OrderCacheObject GetOrderCache(Guid id);
        void Insert(Guid id);
    }

    public class OrderStore : IOrderStore
    {
        
        private readonly Dictionary<Guid, OrderCacheObject> _orderCacheObjects = new Dictionary<Guid, OrderCacheObject>();
     
        public OrderCacheObject GetOrderCache(Guid id)
            => _orderCacheObjects.GetValueOrDefault(id);

        public void Insert(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
