using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using System;

namespace MadXchange.Connector.Messages.Commands
{
    public class CreateOrder : ICommand
    {
        public Guid Id { get; }
        public Exchanges Exchange { get; set; }
        public Guid AccountId { get; set; }       
        public string Symbol { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public OrderType? OrderType { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        public OrderSide Side { get; set; }

        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        
        public CreateOrder(Guid id, Exchanges exchange, Guid accountId, string symbol, decimal? price, decimal? amount, OrderType? type, TimeInForce? tif, OrderSide? side = null)
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            Exchange = exchange;
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

        public CreateOrder(Guid id, IOrderPostRequest request) 
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            Exchange = request.Exchange;
            AccountId = request.AccountId;
            Symbol = request.Symbol;
            Price = request.Price;
            Amount = request.Quantity;
            OrderType = request.OrdType;
            TimeInForce = request.TimeInForce;
            if (Amount.HasValue)
            {
                Side = Amount > 0.0M ? OrderSide.Buy : OrderSide.Sell;
            }
            else
            {
                Side = request.Side.Value;
            }
        }
    }
}
