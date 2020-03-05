using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    [Serializable]
    public class OrderBookCacheObject
    {
        public Guid Id { get; }
        public Xchange Exchange { get; }
        public string Symbol { get; }
        public Dictionary<long, OrderBook> OrderBook { get; } = new Dictionary<long, OrderBook>();
        public long Timestamp { get; internal set; }

        public OrderBookCacheObject(Guid id, Xchange exchange, string symbol)
        {
            Id = id;
            Exchange = exchange;
            Symbol = symbol;
        }

        public void Update(long timeStamp, OrderBook[] insert, OrderBook[] update, OrderBook[] delete)
        {
            Timestamp = timeStamp;
            insert.Each(item => OrderBook.Add(item.Id, item));
            update.Each(item => OrderBook[item.Id].PopulateWithNonDefaultValues(item));
            delete.Each(item => OrderBook.Remove(item.Id));
        }


        public void Insert(long timeStamp, OrderBook[] insert)
        {
            Timestamp = timeStamp;
            insert.Each(item => OrderBook.Add(item.Id, item));
        }
       
    }
}