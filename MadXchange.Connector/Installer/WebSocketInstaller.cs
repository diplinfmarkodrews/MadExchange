using MadXchange.Connector.Services;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.Socket;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MadXchange.Connector.Installer
{
    public static class WebSocketInstaller
    {
        

        public static IServiceCollection AddSocketConnectionService(this IServiceCollection services)
        {
            ////SocketConnection
            services.AddSingleton<ISocketConnectionStore, SocketConnectionStore>();
            services.AddSingleton<ISocketConnectionService, SocketConnectionService>();
            //services.AddScoped<WebSocketConnection>();
            return services;
        }
        
        public static IServiceCollection AddWebSocketHandler(this IServiceCollection services, Assembly assembly = null)
        {

            Assembly ass = assembly ?? typeof(WebSocketHandler).Assembly;
            foreach (var type in ass.GetTypes())
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }
            //foreach(var type in )
            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app)
        {
            return app.Map("wss://", (_app) => _app.UseMiddleware<WebSocketMiddleware>());
        }
    }
}