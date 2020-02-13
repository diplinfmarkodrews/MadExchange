using Convey.Logging;
using MadXchange.Connector.Installer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using ServiceStack;
using System;
using System.IO;
using System.Reflection;

//using ServiceStack.Host.NetCore;

namespace MadXchange.Connector
{
    public class Program
    {

        

        public static int Main(string[] args)
        {
            // IAppHost appHost = new Host(Assembly.GetExecutingAssembly())

            var hostbuilder = CreateHostBuilder(args);
            //MyWebHostExtensions.LogPackagesVersionInfo();
            var host = hostbuilder.Build();
            // 
            host.Run();
            Log.Logger = MyWebHostExtensions.CreateSerilogLogger(MyWebHostExtensions.GetConfiguration());
            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", MyWebHostExtensions.AppName);
                            
                Log.Information("Starting web host ({ApplicationContext})...", MyWebHostExtensions.AppName);
                //host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", MyWebHostExtensions.AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)
                              .UseContentRoot(Directory.GetCurrentDirectory())
                              .UseStartup<Startup>()
                              .UseKestrel()
                              //.UseSockets();//configureOptions: o=> o.NoDelay = true)
                              .UseLogging();
                });
    }
}