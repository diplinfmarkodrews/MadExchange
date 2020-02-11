using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract]
    public class PositionDto : HttpMessageDto
    {
        public Guid AccountId { get; set; }

        [DataMember(IsRequired = true)]
        public string Symbol { get; set; }

        [DataMember(Name = "size")]
        public decimal? CurrentQty { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public decimal? Leverage { get; set; }

        [DataMember]
        public decimal? PositionValue { get; set; }

        [DataMember]
        public decimal? UnrealisedPnl { get; set; }

        [DataMember]
        public decimal? RealisedPnl { get; set; }

        [DataMember(Name = "LiqPrice")]
        public decimal? LiquidationPrice { get; set; }

        [DataMember]
        public decimal? EntryPrice { get; set; }

        [DataMember]
        public decimal? TakeProfit { get; set; }

        [DataMember]
        public decimal? CumRealisedPnl { get; set; }

        [DataMember(Name = "cum_commission")]
        public decimal? CumCommission { get; set; }

        [DataMember]
        public decimal? PositionMargin { get; set; }

        [DataMember]
        public decimal? WalletBalance { get; set; }

        [DataMember]
        public decimal? OrderMargin { get; set; }

        [DataMember]
        public decimal? BustPrice { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }
    }
}