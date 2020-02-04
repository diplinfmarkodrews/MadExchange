
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Instrument", Namespace = "SymbolTicker")]
    public class InstrumentDto : IExtensibleDataObject
    {
        public Exchanges Exchange { get; set; }
        [DataMember(IsRequired = true)]
        public string Symbol { get; set; }
        [DataMember()]
        public decimal? AskPrice { get; set; }
        [DataMember()]
        public decimal? BidPrice { get; set; }        
        [DataMember()]
        public decimal? LastPrice { get; set; }
        [DataMember()]
        public decimal? TickSize { get; set; }
        [DataMember()]
        public decimal? MarkPrice { get; set; }
        [DataMember()]
        public decimal? IndexPrice { get; set; }
        [DataMember()]
        public decimal? OpenValue { get; set; }
        [DataMember()]
        public decimal? OpenInterest { get; set; }
        [DataMember()]
        public decimal? TotalVolume { get; set; }
        [DataMember()]
        public decimal? TotalTurnover { get; set; }
        [DataMember()]
        public decimal? Volume24h { get; set; }
        [DataMember()]
        public decimal? Turnover24h { get; set; }
        [DataMember()]
        public decimal? FundingRate { get; set; }
        [DataMember()]
        public decimal? PredictedFundingRate { get; set; }
        [DataMember()]
        public DateTime NextFundingTime { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
