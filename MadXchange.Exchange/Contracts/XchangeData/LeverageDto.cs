using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Leverage")]
    public class LeverageDto : HttpMessage
    {
        public Guid AccountId { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public decimal Leverage { get; set; }
    }
}