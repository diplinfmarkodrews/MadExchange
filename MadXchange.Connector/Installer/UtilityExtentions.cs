using MadXchange.Connector.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Vault;

namespace MadXchange.Connector.Installer
{
    public static class UtilityExtentions
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