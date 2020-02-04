using MadXchange.Common.Types;
using MadXchange.Connector.Configuration;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Infrastructure.Cache;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace MadXchange.Connector.Installer
{
    public static class CacheInstaller //: IInstaller
    {
        public static IServiceCollection InstallCacheServices(this IServiceCollection services, IConfiguration config)
        {
            var redisCacheSettings = new RedisCacheSettings();
            config.GetSection(key: nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
            //if (!redisCacheSettings.IsEnabled) 
            //{
            //    return services;
            //}
            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IRedisClientsManager, RedisManagerPool>(c=> new RedisManagerPool(redisCacheSettings.ConnectionString));

            services.AddSingleton<IInstrumentCache, InstrumentCache>();
            services.AddSingleton<IAccountRequestCache, AccountRequestCache>();
            services.AddSingleton<IOrderCache, OrderCache>();
            services.AddSingleton<IPositionCache, PositionCache>();
            services.AddSingleton<IMarginCache, MarginCache>();
            return services;
        }
        /// <summary>
        /// Nothing to see here ...
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureCache(this IApplicationBuilder app, IConfiguration config) 
        {
            
            return app;
        }

    }
}
