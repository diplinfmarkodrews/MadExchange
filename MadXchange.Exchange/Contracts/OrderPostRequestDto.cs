using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Contracts
{
    public class OrderPostRequestDto : IOrderPostRequest
    {
        public Guid AccountId { get; private set; }
        public Exchanges Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal? Quantity { get; set; }        
        public OrderSide? Side { get; set; }
        public decimal? Price { get; set; }
        public OrderType? OrdType { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        public IEnumerable<ExecInst> Execs { get; set; }
        
        
    }
}