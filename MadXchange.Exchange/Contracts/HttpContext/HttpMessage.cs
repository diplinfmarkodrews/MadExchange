using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;

namespace MadXchange.Exchange.Contracts
{
    
    public class HttpMessage
    {
        public Xchange Exchange { get; set; }
        public long Timestamp { get; set; } = DateTime.UtcNow.Ticks;
    }
}