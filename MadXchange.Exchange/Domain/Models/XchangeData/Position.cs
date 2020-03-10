using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;

namespace MadXchange.Exchange.Domain.Models
{

    public interface IPosition
    {
        string Symbol { get; }
        public DateTime Created { get; }
        decimal? CurrentQty { get; }
        decimal? PositionValue { get; }
        decimal? Leverage { get; }
        decimal? UnrealisedPnl { get; }
        decimal? RealisedPnl { get; }
        decimal? LiquidationPrice { get; }
        decimal? EntryPrice { get; }
        decimal? PositionMargin { get; }
        decimal? CumRealisedPnl { get; }
        DateTime Updated { get; }
        long Timestamp { get; }
    }

    [Serializable]
    public class Position : IPosition
    {
        public string Symbol { get; set; }
        public decimal? CurrentQty { get; set; }
        public DateTime Created { get; set; }
        public decimal? Leverage { get; set; }
        public decimal? PositionValue { get; set; }
        public decimal? UnrealisedPnl { get; set; }
        public decimal? RealisedPnl { get; set; }
        public decimal? LiquidationPrice { get; set; }
        public decimal? EntryPrice { get; set; }
        public decimal? PositionMargin { get; set; }               
        public decimal? CumRealisedPnl { get; set; }
        public decimal? WalletBalance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public DateTime Updated { get; set; }
        public decimal? MarginBalance { get; set; }
        public string Currency { get; set; }
        public long Timestamp { get; set; }
        public Xchange Exchange { get; private set; }

        public static Position FromModel(MarginDto data)
            => new Position()
            {
                Exchange = data.Exchange,
                Currency = data.Currency,
                WalletBalance = data.WalletBalance,
                AvailableBalance = data.AvailableMargin,
                Updated = data.Updated,
                Timestamp = data.Timestamp
            };

        public static Position[] FromModel(MarginDto[] insert)
        {
            var result = new Position[insert.Length];
            for (int i = 0; i < insert.Length; i++)
                result[i] = FromModel(insert[i]);

            return result;
        }

        public static Position FromModel(PositionDto position)
            => new Position()
            {
                Exchange = position.Exchange,
                Symbol = position.Symbol,
                CurrentQty = position.CurrentQty,
                Created = position.CreatedAt,
                Leverage = position.Leverage,
                PositionValue = position.PositionValue,
                PositionMargin = position.PositionMargin,
                UnrealisedPnl = position.UnrealisedPnl,
                RealisedPnl = position.RealisedPnl,
                LiquidationPrice = position.LiquidationPrice,
                EntryPrice = position.EntryPrice,
                CumRealisedPnl = position.CumRealisedPnl,
                Updated = position.UpdatedAt,
                Timestamp = position.Timestamp,
                WalletBalance = position.WalletBalance,
                AvailableBalance = position.AvailableBalance
            };

        public static Position FromModel(LeverageDto leverage)
            => new Position()
            {
                Exchange = leverage.Exchange,
                Symbol = leverage.Symbol,
                Leverage = leverage.Leverage,
                Timestamp = leverage.Timestamp,
            };

        public static Position[] FromModel(PositionDto[] insert)
        {
            var result = new Position[insert.Length];
            for (int i = 0; i < insert.Length; i++)
                result[i] = FromModel(insert[i]);
            
            return result;
        }
        
    }
}