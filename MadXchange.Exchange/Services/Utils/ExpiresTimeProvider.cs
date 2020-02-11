using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Exchange.Services.Utils
{
    public interface IExpiresTimeProvider
    {
        long Get(Xchange exchange);
    }

    public class ExpiresTimeProvider : IExpiresTimeProvider
    {
        private int LifetimeSeconds = 30;
        private readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ExpiresTimeProvider()
        {
        }

        public long Get(Xchange exchange)
        {
            switch (exchange)
            {
                case Xchange.ByBit:
                    return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                case Xchange.BitMex:
                    return (long)(DateTime.UtcNow - EpochTime).TotalSeconds + LifetimeSeconds;

                default:
                    return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + LifetimeSeconds;
            }
        }
    }
}