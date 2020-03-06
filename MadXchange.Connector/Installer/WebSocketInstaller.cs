using MadXchange.Connector.Services;
using MadXchange.Exchange.Infrastructure.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MadXchange.Connector.Installer
{
    public static class WebSocketInstaller
    {
        
        public static IServiceCollection AddSocketConnectionService(this IServiceCollection services)
        {
            
            services.AddSingleton<ISocketConnectionStore, SocketConnectionStore>();                        
            services.AddTransient<ISocketConnectionService, SocketConnectionService>();
            services.AddTransient<IAccountManager, AccountManager>();
            return services;
        }
              
        public static IApplicationBuilder StartSocketConnections(this IApplicationBuilder app)
        {
            var connectionManager = app.ApplicationServices.GetRequiredService<IAccountManager>();
            connectionManager.RegisterClients();
            return app;
        }
    }
}