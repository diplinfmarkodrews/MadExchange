using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    [DataContract(Name = "Leverage")]
    public class LeverageDto
    {
        public Guid AccountId { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public decimal Leverage { get; set; }

    }
}
