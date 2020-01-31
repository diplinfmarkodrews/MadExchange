using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    [DataContract(Name = "Instrument")]
    public class InstrumentDto
    {
        public Exchanges Exchange { get; set; }
        [DataMember(Name = "symbol", IsRequired = true)]
        public string Symbol { get; private set; }
        [DataMember(Name = "askprice")]
        public decimal? AskPrice { get; private set; }
        [DataMember(Name = "bidprice")]
        public decimal? BidPrice { get; private set; }        
        [DataMember(Name = "lastprice")]
        public decimal? LastPrice { get; private set; }
        [DataMember(Name = "ticksize")]
        public decimal? TickSize { get; private set; }
        [DataMember(Name = "markprice")]
        public decimal? MarkPrice { get; private set; }
        [DataMember(Name = "indexprice")]
        public decimal? IndexPrice { get; private set; }
        [DataMember(Name = "openvalue")]
        public decimal? OpenValue { get; private set; }
        [DataMember(Name = "openinterest")]
        public decimal? OpenInterest { get; private set; }
        [DataMember(Name = "totalvolume")]
        public decimal? TotalVolume { get; private set; }
        [DataMember(Name = "totalturnover")]
        public decimal? TotalTurnover { get; private set; }
        [DataMember(Name = "volume24h")]
        public decimal? Volume24h { get; private set; }
        [DataMember(Name = "turnover24h")]
        public decimal? Turnover24h { get; private set; }
        [DataMember(Name = "fundingrate")]
        public decimal? FundingRate { get; private set; }
        [DataMember(Name = "predictedfundingrate")]
        public decimal? PredictedFundingRate { get; private set; }
        [DataMember(Name = "nextfundingtime")]
        public DateTime NextFundingTime { get; private set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow; 

    }
}
