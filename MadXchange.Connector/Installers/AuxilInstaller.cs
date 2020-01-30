using MadXchange.Connector.Configuration;
//using Microsoft.AspNetCore.Http;
using Convey.HTTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vault;

namespace MadXchange.Connector.Installers
{
    public static class AuxilInstaller
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IHttpClient>("extendedhandlerlifetime").SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            //add http client services
            services.AddHttpClient("GrantClient")
                   .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                   
                    ;
            return services;
        }
        public static IServiceCollection AddVaultService(this IServiceCollection services, IConfiguration configuration)
        {
            var vaultSettings = new VaultSettings();
            configuration.GetSection(key: nameof(VaultSettings)).Bind(vaultSettings);
            services.AddSingleton(vaultSettings);
            services.AddVault();
            return services;
        }
    }
}
