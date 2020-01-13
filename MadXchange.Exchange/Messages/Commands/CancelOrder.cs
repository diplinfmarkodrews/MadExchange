using MadXchange.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Messages.Commands
{
    public class CancelOrder : ICommand
    {
        public Guid Id { get; }
        public Guid OrderId { get; set; }
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public CancelOrder(Guid accountId, Guid orderId, string symbol) 
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            OrderId = orderId;
            Symbol = symbol;
        }
    }
}
