using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using System;
using ServiceStack;

namespace MadXchange.Exchange.Domain.Cache
{
    [Serializable]
    public sealed class InstrumentCacheObject : ICacheObject
    {
        public Xchange Exchange { get; }
        public Guid Id { get; }
        public string Symbol { get; }
        public Instrument Instrument { get; private set; }
        public long Timestamp { get; private set; }
        
        public InstrumentCacheObject(Guid id, Xchange exchange, string symbol)
        {
            Id = id;
            Exchange = exchange;
            Symbol = symbol;
        }

        public bool IsValid() 
            =>
               Instrument is null
            || Instrument.UpdatedAt == default
            || Instrument.Symbol == default
            || Instrument.AskPrice == default
            || Instrument.BidPrice == default
            ? false
            : true;
        
        public void Update(long timeStamp, Instrument instrument) 
        {                        
            Instrument.PopulateWithNonDefaultValues(instrument);
            Timestamp = timeStamp;
        }
    }
}