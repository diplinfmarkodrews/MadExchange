using MadXchange.Common.Types;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Infrastructure.Cache;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace MadXchange.Exchange.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration config)
        {
            var redisCacheSettings = new RedisCacheSettings();
            config.GetSection(key: nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
            if (!redisCacheSettings.IsEnabled) 
            {
                return;
            }
            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IRedisClientsManager>(new RedisManagerPool(redisCacheSettings.ConnectionString));
            services.AddScoped<IInstrumentCache, InstrumentCache>();
            services.AddScoped<IAccountRequestCache, AccountRequestCache>();
            services.AddScoped<IOrderCache, OrderCache>();
            services.AddScoped<IPositionCache, PositionCache>();
            services.AddScoped<IMarginCache, MarginCache>();
        }

        

    }
}
