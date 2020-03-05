using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Contracts.XchangeData;
using System;

namespace MadXchange.Exchange.Domain.Models
{

    public interface IOrderBook
    {
        public long Id { get; }
        public string Symbol { get; }
        public OrderSide? Side { get; }
        public decimal? Size { get; }
        public decimal? Price { get; }
    }

    [Serializable]
    public class OrderBook : IOrderBook
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? Size { get; set; }
        public decimal? Price { get; set; }

        public static OrderBook[] FromModel(OrderBookDto[] data)
        {
            var result = new OrderBook[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = OrderBook.FromModel(data[i]);

            return result;
        }

        public static OrderBook FromModel(OrderBookDto data)
            => new OrderBook() 
            { 
                Id = data.Id, 
            Symbol = data.Symbol, 
             Price = data.Price, 
              Side = data.Side, 
              Size = data.Size 
            };

        
    }
}