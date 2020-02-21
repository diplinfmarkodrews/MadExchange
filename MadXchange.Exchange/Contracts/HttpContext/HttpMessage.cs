using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Exchange.Contracts
{
    
    public class HttpMessage
    {
        public Xchange Exchange { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}