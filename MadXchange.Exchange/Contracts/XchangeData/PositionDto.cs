using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Position")]
    public class PositionDto : HttpMessage
    {
        public Guid AccountId { get; set; }

        [DataMember(IsRequired = true)]
        public string Symbol { get; set; }

        [DataMember]
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

        [DataMember]
        public decimal? LiquidationPrice { get; set; }

        [DataMember]
        public decimal? EntryPrice { get; set; }

        [DataMember]
        public decimal? TakeProfit { get; set; }

        [DataMember]
        public decimal? CumRealisedPnl { get; set; }

        [DataMember]
        public decimal? CumCommission { get; set; }

        [DataMember]
        public decimal? PositionMargin { get; set; }

        /// <summary>
        /// Todo: make these properties type independent or depending on anouther type and route updates to the appropriate Cache channel
        /// </summary>
        [DataMember]
        public decimal? WalletBalance { get; set; }
        [DataMember]
        public decimal? AvailableBalance { get; set; }
        
        [DataMember]
        public decimal? OrderMargin { get; set; }
        /////
        /////
        
        [DataMember]
        public decimal? BustPrice { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }
    }
}