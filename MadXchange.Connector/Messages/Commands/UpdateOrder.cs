using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using System;

namespace MadXchange.Connector.Messages.Commands
{
    public class UpdateOrder : ICommand
    {
        public Guid Id { get; }
        public Exchanges Exchange { get; set; }
        public Guid AccountId { get; set; }
        public string OrderId { get; set; }                
        public string Symbol { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }        
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public UpdateOrder(Guid id, Exchanges exchange, Guid accountId, string orderID, string symbol, decimal? price, decimal? amount)
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            Exchange = exchange;
            AccountId = accountId;
            OrderId = orderID;            
            Symbol = symbol;
            Price = price;
            Amount = amount;            

        }
        public UpdateOrder(Guid id, IOrderPutRequest request)
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            Exchange = request.Exchange;
            AccountId = request.AccountId;
            OrderId = request.OrderId;
            Symbol = request.Symbol;
            Price = request.Price;
            Amount = request.Amount;

        }
    }
}
