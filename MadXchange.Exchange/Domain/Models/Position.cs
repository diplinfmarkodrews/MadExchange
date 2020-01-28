using System;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IPosition
    {
        string Symbol { get; }
        public DateTime Created { get; set; }
        decimal? CurrentQty { get; set; }
        decimal? PositionValue { get; set; }
        decimal? Leverage { get; set; }
        decimal? UnrealisedPnl { get; set; }
        decimal? RealisedPnl { get; set; }
        //OrderSide? Side { get; set; }
        decimal? LiquidationPrice { get; set; }
        decimal? EntryPrice { get; set; }
        decimal? PositionMargin { get; set; }
        decimal? Misc { get; set; }
        decimal? CumRealisedPnl { get; set; } 
        decimal? CumUnRealisedPnl { get; set; }
        DateTime Timestamp { get; set; }

    }

    public class Position : IPosition
    {

        public string Symbol { get; set; }
        public decimal? CurrentQty { get; set; }
        public DateTime Created { get; set; }
        public decimal? Leverage { get; set; }
        public decimal? PositionValue { get; set; }
        public decimal? UnrealisedPnl { get; set; }
        public decimal? RealisedPnl { get; set; }
        //  public OrderSide? Side { get; set; }
        public decimal? LiquidationPrice { get; set; }
        public decimal? EntryPrice { get; set; }
        public decimal? PositionMargin { get; set; }
        public decimal? Misc { get; set; }
        
        public DateTime Timestamp { get; set; }
        public decimal? CumRealisedPnl { get; set; }
        public decimal? CumUnRealisedPnl { get; set; }
       
    }
}
