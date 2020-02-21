using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Order")]
    public class OrderDto : HttpMessage
    {
        public Guid AccountId { get; set; }

        [DataMember]
        public string OrderId { get; set; }

        [DataMember]
        public string ClOrdId { get; set; }

        [DataMember]
        public string ClOrdLinkId { get; set; }

        [DataMember]
        public long? UserId { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public OrderSide? OrderSide { get; set; }

        [DataMember]
        public decimal? OrderQty { get; set; }

        [DataMember]
        public decimal? Price { get; set; }

        [DataMember]
        public OrderStatus? OrdStatus { get; set; }

        [DataMember]
        public DateTimeOffset TransactTime { get; set; }

        [DataMember]
        public decimal? LeavesQty { get; set; }

        [DataMember]
        public decimal? ExecutedQty { get; set; }

        [DataMember]
        public OrderType? OrdType { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public decimal? AvgPx { get; set; }

        public IEnumerable<ExecInst> ExecInst { get; set; }

        [DataMember]
        public string OrdRejReason { get; set; }

        [DataMember]
        public TimeInForce? TimeInForce { get; set; }
    }
}