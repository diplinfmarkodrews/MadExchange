//using Microsoft.AspNetCore.Http;
using MadXchange.Connector.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//using ModernHttpClient;
using ServiceStack;
using Vault;

namespace MadXchange.Connector.Installer
{
    public static class AuxilInstaller
    {
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