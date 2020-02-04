
using MadXchange.Exchange.Domain.Types;
using System;


namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IExpiresTimeProvider
    {
        long Get(Exchanges exchange);
    }

    public class ExpiresTimeProvider : IExpiresTimeProvider
    {
        private int LifetimeSeconds = 30;
        private readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ExpiresTimeProvider() { }                             
        
        public Int64 Get(Exchanges exchange)
        {
            switch (exchange)
            {
                case Exchanges.ByBit:
                    return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
 
                case Exchanges.BitMex:
                    return (long)(DateTime.UtcNow - EpochTime).TotalSeconds + LifetimeSeconds;
                   
                default:
                    return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + LifetimeSeconds;
                 
            }
            
            
        }
    }
}
