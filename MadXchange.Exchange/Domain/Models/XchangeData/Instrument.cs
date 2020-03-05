using System;
using System.Linq;
using MadXchange.Exchange.Contracts;

namespace MadXchange.Exchange.Domain.Models
{

    public interface IInstrument 
    {
    
    }
    [Serializable]
    public sealed class Instrument : IInstrument
    {
        public string Symbol { get; set; }

        public decimal? AskPrice { get; set; }

        public decimal? BidPrice { get; set; }

        public decimal? LastPrice { get; set; }

        public decimal? TickSize { get; set; }

        public decimal? MarkPrice { get; set; }

        public decimal? IndexPrice { get; set; }

        public decimal? OpenValue { get; set; }

        public decimal? OpenInterest { get; set; }

        public decimal? TotalVolume { get; set; }

        public decimal? TotalTurnover { get; set; }

        public decimal? Volume24h { get; set; }

        public decimal? Turnover24h { get; set; }

        public decimal? FundingRate { get; set; }

        public decimal? PredictedFundingRate { get; set; }

        public DateTime NextFundingTime { get; set; }

        public DateTime UpdatedAt { get; set; }

        public static Instrument[] FromModel(InstrumentDto[] models) 
        {
            var result = new Instrument[models.Length];
            for (int i = 0; i < models.Length; i++)            
                result[i] = Instrument.FromModel(models[i]);
            return result;
        }

        public static Instrument FromModel(InstrumentDto model)
            => new Instrument()
            {
                Symbol = model.Symbol,
                AskPrice = model.AskPrice,
                BidPrice = model.BidPrice,
                LastPrice = model.LastPrice,
                TickSize = model.TickSize,
                MarkPrice = model.MarkPrice,
                IndexPrice = model.IndexPrice,
                OpenValue = model.OpenValue,
                OpenInterest = model.OpenInterest,
                TotalVolume = model.TotalVolume,
                TotalTurnover = model.TotalTurnover,
                Volume24h = model.Volume24h,
                Turnover24h = model.Turnover24h,
                FundingRate = model.FundingRate,
                PredictedFundingRate = model.PredictedFundingRate,
                NextFundingTime = model.NextFundingTime,
                UpdatedAt = model.UpdateAt
            }; 
        

        public InstrumentDto ToModel()
        {
            return new InstrumentDto()
            {
                Symbol = Symbol, 
                AskPrice = AskPrice, 
                BidPrice = BidPrice, 
                LastPrice = LastPrice, 
                TickSize = TickSize, 
                MarkPrice = MarkPrice, 
                IndexPrice = IndexPrice, 
                OpenValue = OpenValue, 
                OpenInterest = OpenInterest, 
                TotalVolume = TotalVolume, 
                TotalTurnover = TotalTurnover, 
                Volume24h = Volume24h, 
                Turnover24h = Turnover24h, 
                FundingRate = FundingRate, 
                PredictedFundingRate = PredictedFundingRate, 
                NextFundingTime = NextFundingTime, 
            }; 
        }
    }
}