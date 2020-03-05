using MadXchange.Exchange.Contracts;
using System;

namespace MadXchange.Exchange.Domain.Models.XchangeData
{
    [Serializable]
    public class Execution
    {
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public string OrderId { get; set; }
        public string ExecId { get; set; }
        public string OrderLinkId { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExecQty { get; set; }
        public decimal? LeavesQty { get; set; }
        public bool? IsMaker { get; set; }
        public DateTime? TradeTime { get; set; }
          
    }
}
