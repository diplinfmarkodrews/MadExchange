using System;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IInstrument
    {
        public Guid ID { get; set; }

        public string Symbol
        {
            get; 
        }

        public decimal? HighPrice
        {
            get;
        }

        public decimal? LowPrice
        {
            get;
        }

        public decimal? LastPrice
        {
            get;
        }

        public decimal? AskPrice
        {
            get;
        }

        public decimal? BidPrice
        {
            get;
        }

        public decimal? TickSize
        {
            get;
        }
    }

    public class Instrument : IInstrument
    {
        public Guid ID { get; set; }

        public string Symbol
        {
            get;
            private set;
        }

        public decimal? HighPrice
        {
            get;
            private set;
        }

        public decimal? LowPrice
        {
            get;
            private set;
        }

        public decimal? LastPrice
        {
            get;
            private set;
        }

        //
        public decimal? AskPrice
        {
            get;
            private set;
        }

        public decimal? BidPrice
        {
            get;
            private set;
        }

        public decimal? TickSize
        {
            get;
            set;
        }

        public DateTime Timestamp { get; private set; }
    }
}