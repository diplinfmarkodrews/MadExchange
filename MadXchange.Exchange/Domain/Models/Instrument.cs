using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IInstrument 
    {
        public Guid ID { get; set; }

        public string Symbol
        {
            get;
            set;
        }

        public string RootSymbol
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Typ
        {
            get;
            set;
        }

        public decimal? HighPrice
        {
            get;
            set;
        }

        public decimal? LowPrice
        {
            get;
            set;
        }

        public decimal? LastPrice
        {
            get;
            set;
        }

       
        public decimal? AskPrice
        {
            get;
            set;
        }

        public decimal? BidPrice
        {
            get;
            set;
        }
        public decimal? TickSize
        {
            get;
            set;
        }
 
        public decimal? MakerFee
        {
            get;
            set;
        }
        public decimal? TakerFee
        {
            get;
            set;
        }
    
        public string QuoteCurrency { get; set; }
        public decimal LastBTC
        {
            get;
            set;
        }
       



      
    }


    public class Instrument : IInstrument
    {
        public Guid ID { get; set; }                    

        public string Symbol
        {
            get;
            set;
        }

        public string RootSymbol
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Typ
        {
            get;
            set;
        }

        public decimal? HighPrice
        {
            get;
            set;
        }

        public decimal? LowPrice
        {
            get;
            set;
        }

        public decimal? LastPrice
        {
            get;
            set;
        }

        public decimal? LastChangePcnt
        {
            get;
            set;
        }
        //
        public decimal? AskPrice
        {
            get;
            set;
        }

        public decimal? BidPrice
        {
            get;
            set;
        }
        public decimal? TickSize
        {
            get;
            set;
        }
        public decimal? LastPriceProtected
        {
            get;
            set;
        }

        public decimal? MidPrice
        {
            get;
            set;
        }
        public decimal? MakerFee
        {
            get;
            set;
        }
        public decimal? TakerFee
        {
            get;
            set;
        }
        public decimal? QuoteToSettleMultiplier
        {
            get;
            set;
        }
        public decimal? Multiplier
        {
            get;
            set;
        }
        public decimal? UnderlyingToSettleMultiplier
        {
            get;
            set;
        }
        public decimal? OptionMultiplier
        {
            get;
            set;
        }
        public string QuoteCurrency { get; set; }
        public decimal LastBTC
        {
            get;
            set;
        }

    }
}
