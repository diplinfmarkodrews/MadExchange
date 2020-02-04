using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Margin")]
    public class MarginDto : IMargin
    {
        public Guid AccountId { get; set; }
        [DataMember]
        public string XchangeAccountId { get; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal? WalletBalance { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? MarginBalance { get; set; }
        public decimal? AvailableMargin { get; set; }
    }
}
