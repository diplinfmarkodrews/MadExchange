using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    public interface IOrderPutRequest
    {
        Guid AccountId { get; }
        Exchanges Exchange { get; set; }
        string Symbol { get; set; }
        decimal? Price { get; set; }
        decimal? Qty { get; set; }
        string OrderId { get; set; }

    }

    public interface IOrderPostRequest
    {

        Guid AccountId { get; }
        Exchanges Exchange { get; set; }
        string Symbol { get; }
        decimal? Quantity { get; }
        IEnumerable<ExecInst> Execs { get; set; }
        OrderSide? Side { get; }
        decimal? Price { get; set; }
        OrderType? OrdType { get; set; }
        TimeInForce? TimeInForce { get; set; }
    }
}
