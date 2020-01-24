using MadXchange.Common.Types;
using MadXchange.Connector.Services.Cache;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<ICachedInstrumentService, CachedInstrumentService>();
        }

        

    }
}
