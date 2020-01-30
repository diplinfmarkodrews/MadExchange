using CacheManager.Core;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{

    public class CacheKeeper<T>
    {
        private readonly ICacheManager<T> _distributed;
        private readonly ICacheManager<T> _inMemory;
        private bool _distributedEnabled = true;

        public CacheKeeper()
        {
            var multiplexer = ConnectionMultiplexer.Connect("localhost");

            multiplexer.ConnectionFailed += (sender, args) =>
            {
                _distributedEnabled = false;

                Console.WriteLine("Connection failed, disabling redis...");
            };

            multiplexer.ConnectionRestored += (sender, args) =>
            {
                _distributedEnabled = true;

                Console.WriteLine("Connection restored, redis is back...");
            };

            _distributed = CacheFactory.Build<T>(
                s => s
                    
                    .WithDictionaryHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.MaxValue)
                    .And
                    .WithRedisConfiguration("redis", multiplexer)
                    .WithRedisCacheHandle("redis"));

            _inMemory = CacheFactory.Build<T>(
                s => s
                    .WithDictionaryHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.MaxValue));
        }

        public ICacheManager<T> Cache
        {
            get
            {
                if (_distributedEnabled)
                {
                    return _distributed;
                }

                return _inMemory;
            }
        }
    }
    
}
