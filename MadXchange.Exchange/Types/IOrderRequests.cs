using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Types
{
    public interface IOrderPutRequest
    {
        Guid AccountId { get; }
        Xchange Exchange { get; set; }
        string Symbol { get; set; }
        decimal? Price { get; set; }
        decimal? Qty { get; set; }
        string OrderId { get; set; }
    }

    public interface IOrderPostRequest
    {
        Guid AccountId { get; }
        Xchange Exchange { get; set; }
        string Symbol { get; }
        decimal? Quantity { get; }
        IEnumerable<ExecInst> Execs { get; set; }
        OrderSide? Side { get; }
        decimal? Price { get; set; }
        OrderType? OrdType { get; set; }
        TimeInForce? TimeInForce { get; set; }
    }
}