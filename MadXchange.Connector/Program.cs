using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using ServiceStack;
using System;
using System.IO;
using ServiceStack.Host.NetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore;

namespace MadXchange.Connector
{
    public class Program
    {
        
        public static int Main(string[] args)
        {
                                
            Log.Logger = MyWebHostExtensions.CreateSerilogLogger(MyWebHostExtensions.GetConfiguration());           
            try
            {
                
                var hostbuilder = CreateHostBuilder(args);
                MyWebHostExtensions.LogPackagesVersionInfo();
                Log.Information("Configuring web host ({ApplicationContext})...", MyWebHostExtensions.AppName);
                var host = hostbuilder.Build();
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = "http://localhost:5000/";
                proc.Start();
                Log.Information("Starting web host ({ApplicationContext})...", MyWebHostExtensions.AppName);
                host.Run();               
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

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)                                 
                   .UseSerilog(Log.Logger)
                   .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                   .CaptureStartupErrors(true)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseKestrel()
                   .UseSockets(configureOptions: o => o.NoDelay = true);
                              
                
    
    }
    
}

/*
 
      public class AppHost : AppSelfHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("ServiceStackHostEnvironment", typeof(MyServices).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }
    }*/
