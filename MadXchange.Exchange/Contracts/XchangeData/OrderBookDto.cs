using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts.XchangeData
{
    [DataContract]
    public class OrderBookDto
    {
        public Xchange Exchange { get; set; }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public decimal? Size { get; set; }
        [DataMember]
        public OrderSide? Side { get; set; }

    }
}
