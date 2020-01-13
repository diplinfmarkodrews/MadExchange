using MadXchange.Exchange.Domain.Models;
using System;


namespace MadXchange.Common.Mex
{
    public interface IExpiresTimeProvider
    {
        Int64 Get();
    }

    public class ExpiresTimeProvider : IExpiresTimeProvider
    {
        private int LifetimeSeconds = 30;

        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private Exchanges exchange;

        public ExpiresTimeProvider(Exchanges exchange)
        {
            this.exchange = exchange;
            switch (exchange) 
            {
                case Exchanges.ByBit:
                    LifetimeSeconds = 5000;
                    break;
                case Exchanges.BitMex:
                    LifetimeSeconds = 30;
                    break;
                default:break;
            }

        }

        public Int64 Get()
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
