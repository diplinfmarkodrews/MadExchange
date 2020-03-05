using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{

    public interface IOrderBookCache
    {        
        public Task<Dictionary<long, OrderBook>> GetOrderBookAsync(Guid id, Xchange exchange, string symbol);
        public long Update(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert, OrderBook[] update, OrderBook[] delete);
        public long InsertOrderBook(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert);
    }


    public class OrderBookCache : CacheStorage<OrderBookCacheObject>, IOrderBookCache
    {
        private IOrderBookStore _orderBookStore = new OrderBookStore();

        public OrderBookCache(IRedisClientsManager cacheClientManager) : base("OrderBook", cacheClientManager) { }
       
        private string CreateKeyString(Guid id, Xchange exchange, string symbol)
           => $"{exchange}{symbol}{id}";
        /// <summary>
        /// Updates Instrument CacheInstances
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="orderBookDto"></param>
        /// <returns></returns>
        public long Update(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] inserts, OrderBook[] updates, OrderBook[] deletes)
        {
            var keyString = CreateKeyString(id, exchange, symbol);
            var cacheObject = _orderBookStore.GetOrderBook(keyString, id, exchange, symbol);
            cacheObject.Update(timeStamp: timeStamp, 
                                  insert: inserts,
                                  update: updates,
                                  delete: deletes);
            Set(keyString, cacheObject);
            return cacheObject.Timestamp;
        }
       

        public async Task UpdateAsync(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] inserts, OrderBook[] updates, OrderBook[] deletes)
        {
            var keyString = CreateKeyString(id, exchange, symbol);
            var cacheObject = _orderBookStore.GetOrderBook(keyString, id, exchange, symbol);
            cacheObject.Update(timeStamp: timeStamp,
                                  insert: inserts,
                                  update: updates,
                                  delete: deletes);

            await SetAsync(keyString, cacheObject);
            return;
        }

        public long InsertOrderBook(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] snapShot)
        {
            var keyString = CreateKeyString(id, exchange, symbol);
            var cacheObject = _orderBookStore.GetOrderBook(keyString, id, exchange, symbol);
            cacheObject.Insert(timeStamp, snapShot);
            _orderBookStore.SetCacheObject(keyString, cacheObject);
            Set(keyString, cacheObject);
            return cacheObject.Timestamp;
        }

        public async Task<Dictionary<long, OrderBook>> GetOrderBookAsync(Guid id, Xchange exchange, string symbol)
        {
            return (await GetAsync(CreateKeyString(id, exchange, symbol))).OrderBook;
        }
    }
}
