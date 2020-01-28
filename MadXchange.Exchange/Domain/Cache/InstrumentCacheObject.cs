using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public class InstrumentCacheObject : ICacheObject
    {
        public Exchanges Exchange { get; }       
        public Instrument Instrument { get; set; }
        public bool IsValid() => 
               Instrument is null 
            || Instrument.Timestamp == default 
            || Instrument.Symbol == default 
            || Instrument.AskPrice == default 
            || Instrument.BidPrice == default 
            ? false 
            : true;
    }
}
