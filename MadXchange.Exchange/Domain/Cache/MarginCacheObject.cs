using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public class MarginCacheObject
    {
        
        public Guid AccountId { get; set; }        
        public Margin MarginObj { get; set; }
    }
}
