using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Execution")]
    public class ExecutionDto : HttpMessage
    {
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public OrderSide? OrderSide { get; set; }
        [DataMember]
        public string OrderId { get; set; }
        [DataMember]
        public string ExecId { get; set; }
        [DataMember]
        public string OrderLinkId { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public decimal? ExecQty { get; set; }
        [DataMember]
        public decimal? ExecFee { get; set; }
        [DataMember]
        public decimal? LeavesQty { get; set; }
        [DataMember]
        public bool? IsMaker { get; set; }
        [DataMember]
        public DateTime XTimestamp { get; set; }
          
    }
}
