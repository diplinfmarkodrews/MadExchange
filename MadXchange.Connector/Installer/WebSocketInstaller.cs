using MadXchange.Connector.Services;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.Socket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace MadXchange.Connector.Installer
{
    public static class WebSocketInstaller
    {
        
        public static IServiceCollection AddSocketConnectionService(this IServiceCollection services)
        {
            ////SocketConnection
            //services.AddSession();
           // services.AddConnections();
            //services.AddHttpContextAccessor();
            services.AddSingleton<ISocketConnectionStore, SocketConnectionStore>();                       
 
            services.AddTransient<ISocketConnectionService, SocketConnectionService>();
            services.AddTransient<IAccountManager, AccountManager>();
           
            

            //services.AddTransient<WebSocketMiddleware>();
           // services.AddScoped<IAddressProvider, AddressProvider>();
            return services;
        }
        
        //public static IServiceCollection AddWebSocketHandler(this IServiceCollection services, Assembly assembly = null)
        //{

        //    Assembly ass = assembly ?? typeof(WebSocketHandler).Assembly;
        //    foreach (var type in ass.GetTypes())
        //    {
        //        if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
        //        {
        //            services.AddSingleton(type);
        //        }
        //    }
        //    //foreach(var type in )
        //    return services;
        //}

       
        public static IApplicationBuilder StartSocketConnections(this IApplicationBuilder app)
        {
            var connectionManager = app.ApplicationServices.GetRequiredService<IAccountManager>();
            connectionManager.RegisterClients();
            return app;
        }
    }
}