using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MadXchange.Connector
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupTcpService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ServerConfig>(configuration);
            services.AddSingleton<ISocketService, TcpService>();
            services.AddScoped<IClientSocket, ClientSocket>();
            return services;
        }
    }
}
