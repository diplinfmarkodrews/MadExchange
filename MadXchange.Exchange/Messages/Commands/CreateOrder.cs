using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Messages.Commands
{
    public class CreateOrder : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ExchangeId { get; set; }
        public string Symbol { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public OrderType? OrderType { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        public OrderSide Side { get; set; }

        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public CreateOrder(Guid accountId, string symbol, decimal? price, decimal? amount, OrderType? type, TimeInForce? tif, OrderSide? side = null)
        {
            Id = Guid.NewGuid();            
            AccountId = accountId;
            Symbol = symbol;
            Price = price;
            Amount = amount;
            OrderType = type;
            TimeInForce = tif;
            if (amount.HasValue)
            {
                Side = amount > 0.0M ? OrderSide.Buy : OrderSide.Sell;
            }
            else 
            {
                Side = side.Value;
            }

        }
    }
}
