using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Infrastructure.Stores
{

    public interface IOrderBookStore
    {
        OrderBookCacheObject GetOrderBook(string key, Guid id, Xchange exchange, string symbol);
        void SetCacheObject(string key, OrderBookCacheObject cacheObject);
    }

    public class OrderBookStore : IOrderBookStore
    {
        private readonly Dictionary<string, OrderBookCacheObject> _orderBookCacheObjects = new Dictionary<string, OrderBookCacheObject>();

        public OrderBookCacheObject GetOrderBook(string key, Guid id, Xchange exchange, string symbol)
            => _orderBookCacheObjects.GetValueOrDefault(key, new OrderBookCacheObject(id, exchange, symbol));

        public void SetCacheObject(string key, OrderBookCacheObject cacheObject)
            => _orderBookCacheObjects[key] = cacheObject;
    }
}
