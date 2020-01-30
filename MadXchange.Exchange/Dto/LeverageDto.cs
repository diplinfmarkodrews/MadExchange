using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    public class LeverageDto
    {
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public decimal Leverage { get; set; }
    }
}
