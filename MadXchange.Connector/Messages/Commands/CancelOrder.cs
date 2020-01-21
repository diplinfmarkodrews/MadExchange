using MadXchange.Common.Messages;
using System;

namespace MadXchange.Connector.Messages.Commands
{
    public class CancelOrder : ICommand
    {
        public Guid Id { get; }
        public int ExchangeId { get; set; }
        public Guid OrderId { get; set; }
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public CancelOrder(Guid id, int exchangeId, Guid accountId, Guid orderId, string symbol) 
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            ExchangeId = exchangeId;
            AccountId = accountId;
            OrderId = orderId;
            Symbol = symbol;
        }
    }
}
