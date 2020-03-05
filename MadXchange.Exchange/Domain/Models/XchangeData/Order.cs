
using Convey.Types;
using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IOrder 
    {
        public string OrderId { get; }
        public long? UserId { get; }
        public string Symbol { get; }
        public OrderSide? Side { get; }
        public decimal? OrderQty { get; }
        public decimal? Price { get; }
        public OrderStatus? OrdStatus { get; }
        public long TransactTime { get; }
        public long Timestamp { get; }
        public OrderType? OrdType { get; }
        public string Text { get; }
        public decimal? AvgPx { get; }
        public IEnumerable<ExecInst> ExecInst { get; }
        public string OrdRejReason { get; }
        public TimeInForce? TimeInForce { get; }
       

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
    [Serializable]
    public class Order : IOrder
    {
        public Guid Id { get; }
        public string OrderId { get; set; }
        public long? UserId { get; set; }
        public string Symbol { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? Price { get; set; }
        public OrderStatus? OrdStatus { get; set; }
        public long TransactTime { get; set; }        
        public decimal? LeavesQty { get; set; }
        public decimal? ExecutedQty { get; set; }
        public OrderType? OrdType { get; set; }
        public string Text { get; set; }
        public decimal? AvgPx { get; set; }
        public IEnumerable<ExecInst> ExecInst { get; set; }
        public string OrdRejReason { get; set; }
        public TimeInForce? TimeInForce { get; set; }

        public long Timestamp { get; set; }
        

        public static Order FromModel(OrderDto data)
            => new Order()
            {
                Symbol = data.Symbol,
                OrderId = data.OrderId,
                UserId = data.UserId,
                Side = data.OrderSide,
                OrderQty = data.OrderQty,
                Price = data.Price,
                OrdStatus = data.OrdStatus,
                LeavesQty = data.LeavesQty,
                ExecutedQty = data.ExecutedQty,
                OrdType = data.OrdType,
                Text = data.Text,
                AvgPx = data.AvgPx,
                OrdRejReason = data.OrdRejReason,
                TimeInForce = data.TimeInForce,
                TransactTime = data.TransactTime.ToUnixTimeSeconds(),
                Timestamp = data.Timestamp,
                // tado add OrderProperties
                ExecInst = new List<ExecInst>() { }

            };
        
        public static Order[] FromModel(OrderDto[] datas) 
        {
            var result = new Order[datas.Length];
            for (int i = 0; i < datas.Length; i++)
                result[i] = FromModel(datas[i]);

            return result;
        }

        public bool IsReduceOnly()
            => ExecInst.Any(p => p == Contracts.ExecInst.ReduceOnly);

        public bool IsPostOnly()
            => ExecInst.Any(p => p == Contracts.ExecInst.ParticipateDoNotInitiate);
        

        public bool IsClose()
            => ExecInst.Any(p => p == Contracts.ExecInst.Close);


        public bool IsPegPriceOrder()
            => (OrdType == OrderType.Stop || 
                OrdType == OrderType.MarketIfTouched) 
                ? true
                : false;

        public bool IsOrderNewOrPartiallyFilled()
            => (OrdStatus == OrderStatus.New || 
                OrdStatus == OrderStatus.PartiallyFilled) 
                ? true
                : false;

        public bool IsOrderFilled()
            => (OrdStatus == OrderStatus.Filled) ? true : false;

        public bool IsOrderTerminated()
            => (OrdStatus == OrderStatus.Filled   ||
                OrdStatus == OrderStatus.Rejected ||
                OrdStatus == OrderStatus.Canceled ||
                OrdStatus == OrderStatus.Expired  ||
                OrdStatus == OrderStatus.Stopped) 
                ? true 
                : false;

        public bool IsOrderAborted()
            => (OrdStatus == OrderStatus.Rejected ||
                OrdStatus == OrderStatus.Canceled ||
                OrdStatus == OrderStatus.Expired ||
                OrdStatus == OrderStatus.Stopped)
                ? true
                : false;

        public bool IsOrderPartiallyFilled()
            => (OrdStatus == OrderStatus.PartiallyFilled)
                ? true
                : false;

        public bool IsOrderNew()
            => (OrdStatus == OrderStatus.New)
                ? true
                : false;
            
    }
}