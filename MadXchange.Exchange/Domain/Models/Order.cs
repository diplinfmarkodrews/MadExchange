using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using MadXchange.Common.Types;
using Convey.Types;

namespace MadXchange.Exchange.Domain.Models
{
    public enum OrderStatus
    {
        UNKNOWN = 0,
        CANCELED = 1,
        EXPIRED = 2,
        FILLED = 3,
        NEW = 4,
        PARTIALLYCANCELED = 5,
        PARTIALLYFILLED = 6,
        PENDINGCANCEL = 7,
        PENDINGNEW = 8,
        PENDINGREPLACE = 9,
        REJECTED = 10,
        REPLACED = 11,
        STOPPED = 12
    }
    public enum OrderSide
    {
        Buy = 0,
        Sell = 1
    }
    public enum OrderType
    {

        Limit = 0, //these 2 needed for binance only
        Market = 1, //
        Stop = 2,
        StopLimit = 3,
        MarketIfTouched = 4,
        LimitIfTouched = 5,
        MarketWithLeftOverAsLimit = 6,
        Pegged = 7,
        Unknown = 9

    }
    public enum TimeInForce
    {
        
        GTC = 0, //0 is standard
        IOC = 1,
        FOK = 3,
        
    }

    public enum ExecInst
    {
        ParticipateDoNotInitiate,
        AllOrNone,
        MarkPrice,
        IndexPrice,
        LastPrice,
        Close,
        ReduceOnly,
        Fixed
    }
    public interface IOrder : IIdentifiable<Guid>
    {
        
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
       

        public Order(

            )
        {




        }



        public bool IsReduceOnly() 
        {
            return this.ExecInst.Any(p=>p == Models.ExecInst.ReduceOnly);
        }

        public bool IsPostOnly() 
        {
            return this.ExecInst.Any(p=>p == Models.ExecInst.ParticipateDoNotInitiate);
        }

        public bool IsClose() 
        {
            return ExecInst.Any(p=>p == Models.ExecInst.Close);
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

            if (OrdStatus == OrderStatus.NEW || OrdStatus == OrderStatus.PARTIALLYFILLED)
            {
                return true;
            }
            return false;
        }
        public bool IsOrderFilled()
        {

            if (OrdStatus == OrderStatus.FILLED)
            {
                return true;
            }
            return false;
        }
        public bool IsOrderTerminated()
        {

            if (OrdStatus == OrderStatus.FILLED || OrdStatus == OrderStatus.REJECTED || OrdStatus == OrderStatus.CANCELED || OrdStatus == OrderStatus.EXPIRED || OrdStatus == OrderStatus.STOPPED)
            {
                return true;
            }
            return false;
        }
        public bool IsOrderAborted()
        {

            if (OrdStatus == OrderStatus.REJECTED || OrdStatus == OrderStatus.CANCELED || OrdStatus == OrderStatus.EXPIRED || OrdStatus == OrderStatus.STOPPED)
            {
                return true;
            }
            return false;
        }
        public bool IsOrderPartiallyFilled()
        {

            if (OrdStatus == OrderStatus.PARTIALLYFILLED)
            {
                return true;
            }
            return false;
        }
        public bool IsOrderNew()
        {

            if (OrdStatus == OrderStatus.NEW)
            {
                return true;
            }
            return false;
        }
    }
    
}
