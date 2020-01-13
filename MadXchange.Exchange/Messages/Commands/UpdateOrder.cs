using MadXchange.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Messages.Commands
{
    public class UpdateOrder : ICommand
    {
        public Guid Id { get; }
        public Guid OrderId { get; set; }
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }        
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public UpdateOrder(Guid accountId, Guid orderID, string symbol, decimal? price, decimal? amount)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            OrderId = orderID;
            Symbol = symbol;
            Price = price;
            Amount = amount;            

        }
    }
}
