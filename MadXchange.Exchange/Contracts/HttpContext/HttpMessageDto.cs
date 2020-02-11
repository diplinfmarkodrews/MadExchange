using System;

namespace MadXchange.Exchange.Contracts
{
    public class HttpMessageDto
    {
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}