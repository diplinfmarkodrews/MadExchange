//using Microsoft.AspNetCore.Http;
using Convey.HTTP;
using MadXchange.Connector.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModernHttpClient;
using ServiceStack;
using System;
using System.Threading;
using Vault;

namespace MadXchange.Connector.Installer
{
    public static class AuxilInstaller
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IHttpClient>(cfg => {
                        JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler(throwOnCaptiveNetwork: true, customSSLVerification: false);
                        JsonHttpClient.DefaultUserAgent = "MadMexIO";               
                    })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //PollyServiceCollectionExtensions.AddPolicyRegistry(services);

            //add http client services
            //services.AddHttpClient("GrantClient")
            //       .SetHandlerLifetime(TimeSpan.FromMinutes(5))                   
            //        ;
           
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
