using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Order")]
    public class OrderDto 
    {
        public Guid AccountId { get; set; }
        [DataMember]
        public string OrderId { get; set; }
        public string ClOrdId { get; set; }
        public string ClOrdLinkId { get; set; }
        public long? Account { get; set; }
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? Price { get; set; }
        public OrderStatus? OrdStatus { get; set; }
        public DateTimeOffset TransactTime { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal? LeavesQty { get; set; }
        public decimal? ExecutedQty { get; set; }
        public OrderType? OrdType { get; set; }
        public string Text { get; set; }
        public decimal? AvgPx { get; set; }
        public IEnumerable<ExecInst> ExecInst { get; set; }
        public string OrdRejReason { get; set; }
        public TimeInForce? TimeInForce { get; set; }
    }
}
