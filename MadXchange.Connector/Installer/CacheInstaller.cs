using MadXchange.Connector.Configuration;
using MadXchange.Exchange.Handler;
using MadXchange.Exchange.Infrastructure.Cache;
using MadXchange.Exchange.Infrastructure.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace MadXchange.Connector.Installer
{
    public static class CacheInstaller //: IInstaller
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration config)
        {
          
            var redisCacheSettings = new RedisCacheSettings();
            config.GetSection(key: "Redis").Bind(redisCacheSettings);            
            services.AddSingleton(redisCacheSettings);           
            services.AddSingleton<IRedisClientsManager, RedisManagerPool>(c => new RedisManagerPool(redisCacheSettings.ConnectionString));
            services.AddTransient<IRedisClient, RedisClient>();
            
            services.AddTransient<IAccountRequestCache, AccountRequestCache>();
            
            services.AddSingleton<IOrderBookCache, OrderBookCache>();
            services.AddSingleton<IOrderCache, OrderCache>();
            services.AddSingleton<IPositionCache, PositionCache>();
            services.AddSingleton<IInstrumentCache, InstrumentCache>();
            services.AddSingleton<IConnectionDataHandler, CacheDataHandler>();
            
            
            
          

            return services;
        }

        /// <summary>
        /// Nothing to see here ...
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        //public static IApplicationBuilder ConfigureCache(this IApplicationBuilder app)
        //{
        //    var redisClient = app.ApplicationServices.GetRequiredService<IRedisClient>();
        //    var allkeys = redisClient.GetAllKeys();
            
        //    return app;
        //}
    }
}