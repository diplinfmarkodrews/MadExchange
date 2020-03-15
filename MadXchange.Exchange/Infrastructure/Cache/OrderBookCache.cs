using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Types;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{

    public interface IOrderBookCache
    {        
        Task<Dictionary<long, OrderBook>> GetAsync(Guid id, Xchange exchange, string symbol);
        Dictionary<long, OrderBook> Get(Guid id, Xchange exchange, string symbol);
        Task UpdateAsync(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] inserts, OrderBook[] updates, OrderBook[] deletes);
        void Update(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert, OrderBook[] update, OrderBook[] delete);
        
        void Insert(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert);
        void Store();
    }


    public class OrderBookCache : CacheStorageTransient<OrderBookCacheObject>, IOrderBookCache
    {
        private readonly IOrderBookStore _orderBookStore = new OrderBookStore();

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
        private OrderBookCacheObject Update(string keyString, Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] inserts, OrderBook[] updates, OrderBook[] deletes)        
            => _orderBookStore.GetOrderBook(key: keyString, id, exchange, symbol)
                              .Update(timeStamp: timeStamp,
                                         insert: inserts,
                                         update: updates,
                                         delete: deletes);

        private OrderBookCacheObject Insert(string keyString, Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert)
            => _orderBookStore.GetOrderBook(key: keyString, id, exchange, symbol)
                              .Insert(timeStamp, insert);

        public async Task UpdateAsync(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] inserts, OrderBook[] updates, OrderBook[] deletes)
        {
            var keyString = CreateKeyString(id, exchange, symbol);
            var cacheObject = Update(keyString: keyString,
                                            id: id,
                                      exchange: exchange,
                                        symbol: symbol,
                                     timeStamp: timeStamp,
                                       inserts: inserts,
                                       updates: updates,
                                       deletes: deletes);

            await SetAsync(keyString, cacheObject);
            return;
        }

        public void Insert(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] snapShot)
            => Insert(CreateKeyString(id, exchange, symbol), id, exchange, symbol, timeStamp, snapShot);                                                  
        

        public Dictionary<long, OrderBook> Get(Guid id, Xchange exchange, string symbol)
            => _orderBookStore.GetOrderBook(key: CreateKeyString(id, exchange, symbol), 
                                             id: id, 
                                       exchange: exchange, 
                                         symbol: symbol)?.OrderBook;
        
        public async Task<Dictionary<long, OrderBook>> GetAsync(Guid id, Xchange exchange, string symbol)
            => (await GetAsync(CreateKeyString(id, exchange, symbol)))?.OrderBook;

        public void Update(Guid id, Xchange exchange, string symbol, long timeStamp, OrderBook[] insert, OrderBook[] update, OrderBook[] delete)
         => Update(CreateKeyString(id, exchange, symbol), id, exchange, symbol, timeStamp, insert, update, delete);

        public void Store()
        {
            throw new NotImplementedException();
        }
    }
}
