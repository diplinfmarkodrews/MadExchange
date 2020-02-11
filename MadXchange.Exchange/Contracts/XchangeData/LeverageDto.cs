using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract]
    public class LeverageDto : HttpMessageDto
    {
        public Guid AccountId { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public decimal Leverage { get; set; }
    }
}