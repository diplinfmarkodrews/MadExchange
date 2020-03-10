using Convey.CQRS.Commands;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;

namespace MadXchange.Exchange.Messages.Commands.OrderService
{
    public class CancelOrder : ICommand
    {
        public Guid Id { get; }
        public Xchange Exchange { get; set; }
        public string OrderId { get; set; }
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public CancelOrder(Guid id, Xchange exchange, Guid accountId, string orderId, string symbol)
        {
            Id = id == default ? Guid.NewGuid() : id;
            Exchange = exchange;
            AccountId = accountId;
            OrderId = orderId;
            Symbol = symbol;
        }

       
    }
}