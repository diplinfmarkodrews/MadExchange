
using Convey.Types;
using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IOrder : IIdentifiable<Guid>
    {
        public string OrderId { get; set; }
        public long? Account { get; set; }
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? Price { get; set; }
        public OrderStatus? OrdStatus { get; set; }
        public long TransactTime { get; set; }
        public long Timestamp { get; set; }
        public OrderType? OrdType { get; set; }
        public string Text { get; set; }
        public decimal? AvgPx { get; set; }
        public IEnumerable<ExecInst> ExecInst { get; set; }
        public string OrdRejReason { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        Guid Id { get; }

        bool IsOrderNewOrPartiallyFilled();

        bool IsPegPriceOrder();

        bool IsOrderFilled();

        bool IsOrderTerminated();

        bool IsOrderAborted();

        bool IsOrderNew();

        bool IsOrderPartiallyFilled();

        bool IsPostOnly();

        bool IsReduceOnly();

        bool IsClose();
    }

    public class Order : IOrder, IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public string OrderId { get; set; }
        public long? Account { get; set; }
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? Price { get; set; }
        public OrderStatus? OrdStatus { get; set; }
        public long TransactTime { get; set; }
        public long Timestamp { get; set; }
        public decimal? LeavesQty { get; set; }
        public decimal? ExecutedQty { get; set; }
        public OrderType? OrdType { get; set; }
        public string Text { get; set; }
        public decimal? AvgPx { get; set; }
        public IEnumerable<ExecInst> ExecInst { get; set; }
        public string OrdRejReason { get; set; }
        public TimeInForce? TimeInForce { get; set; }

        public Order()
        {
        }

        public bool IsReduceOnly()
        {
            return this.ExecInst.Any(p => p == Contracts.ExecInst.ReduceOnly);
        }

        public bool IsPostOnly()
        {
            return this.ExecInst.Any(p => p == Contracts.ExecInst.ParticipateDoNotInitiate);
        }

        public bool IsClose()
        {
            return ExecInst.Any(p => p == Contracts.ExecInst.Close);
        }

        public bool IsPegPriceOrder()
        {
            if (OrdType == OrderType.Stop || OrdType == OrderType.MarketIfTouched)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderNewOrPartiallyFilled()
        {
            if (OrdStatus == OrderStatus.New || OrdStatus == OrderStatus.PartiallyFilled)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderFilled()
        {
            if (OrdStatus == OrderStatus.Filled)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderTerminated()
        {
            if (OrdStatus == OrderStatus.Filled || OrdStatus == OrderStatus.Rejected || OrdStatus == OrderStatus.Canceled || OrdStatus == OrderStatus.Expired || OrdStatus == OrderStatus.Stopped)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderAborted()
        {
            if (OrdStatus == OrderStatus.Rejected || OrdStatus == OrderStatus.Canceled || OrdStatus == OrderStatus.Expired || OrdStatus == OrderStatus.Stopped)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderPartiallyFilled()
        {
            if (OrdStatus == OrderStatus.PartiallyFilled)
            {
                return true;
            }
            return false;
        }

        public bool IsOrderNew()
        {
            if (OrdStatus == OrderStatus.New)
            {
                return true;
            }
            return false;
        }
    }
}