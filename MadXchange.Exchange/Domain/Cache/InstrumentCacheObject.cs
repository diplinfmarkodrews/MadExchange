using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    [Serializable]
    public class InstrumentCacheObject : ICacheObject
    {
        public Xchange Exchange { get; }

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