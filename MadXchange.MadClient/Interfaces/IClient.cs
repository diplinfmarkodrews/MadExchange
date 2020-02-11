using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Common.Mex.Infrastructure
{
    public interface IClient : IDisposable
    {
        Guid AccuntId { get; }
        //event EventHandler<ClientFailEvent> ClientFailEvent;
    }

    public class ClientFailEvent : EventArgs
    {
        public Guid AccountId { get; }
        public Exception Exception { get; set; }
    }

    public class LiquidationEvent : EventArgs
    {
        public Guid AccountId { get; }
        public string Symbol { get; }
    }

    public class PositionEvent : EventArgs
    {
        public Guid AccountId { get; }
        public IPosition Position { get; set; }
        public IMargin Margin { get; set; }

        public PositionEvent(IPosition position = null, IMargin margin = null)
        {
            Position = position;
            Margin = margin;
        }
    }

    public class OrderEvent : EventArgs
    {
        public Guid AccountId { get; }
        public string Symbol { get; }
        public List<IOrder> Orders { get; set; }

        public bool LastOrder { get; set; } = true;

        public OrderEvent(IOrder order, Guid accountId)
        {
            AccountId = accountId;
            Orders = new List<IOrder>();
            Orders.Add(order);
            LastOrder = false;
        }

        public OrderEvent(List<IOrder> orders, Guid accountId)
        {
            AccountId = accountId;
            Orders = orders;
            Symbol = orders.FirstOrDefault().Symbol;
            LastOrder = false;
        }

        public OrderEvent(List<IOrder> orders, Guid accountId, bool last)
        {
            AccountId = accountId;
            Symbol = orders.FirstOrDefault().Symbol;
            Orders = orders;
            LastOrder = last;
        }
    }
}