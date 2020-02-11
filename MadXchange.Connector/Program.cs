using Convey.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using System.IO;

//using ServiceStack.Host.NetCore;

namespace MadXchange.Connector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // IAppHost appHost = new Hosy(Assembly.GetExecutingAssembly())
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)
                              .UseContentRoot(Directory.GetCurrentDirectory())
                              .UseStartup<Startup>()
                              .UseKestrel()                              
                              .UseSockets()//configureOptions: o=> o.NoDelay = true)
                              .UseLogging();
                });
    }
}