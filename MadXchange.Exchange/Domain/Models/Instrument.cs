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
            
        }

      
        public string Typ
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
 
        public decimal? MakerFee
        {
            get;
          
        }
        public decimal? TakerFee
        {
            get;
            
        }
    
        
        public decimal LastBTC
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
        

        public string State
        {
            get;
            set;
        }

        public string Typ
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
                
        public decimal LastBTC
        {
            get;
            set;
        }

        public DateTime Timestamp { get; private set; }

    }
}
