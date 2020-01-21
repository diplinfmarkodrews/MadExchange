using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Net.WebSockets;
using Convey;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Configuration;
using Autofac;
using System.IO;
using MadXchange.Common.Types;
using MadXchange.Exchange.Installers;
using MadXchange.Connector.Services;
using Vault;
using Convey.Persistence.MongoDB;

namespace MadXchange.Connector
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }

        public IContainer Container { get; set; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddJsonFile("exchangesettings.json", optional: false, reloadOnChange: false);
            
            Configuration = builder.Build();
            
            services.AddConvey("connector");
            services.InstallExchangeDescriptorDictionary(Configuration);
            services.AddSingleton<IExchangeDescriptorService>();
            services.AddPolicyRegistry();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            //services.AddOpenTracingCoreServices();
            //services.AddWebEncoders();
            services.AddMetrics().AddMetricsEndpoints();

            //services.AddVault();
            services.AddLogging(logBuilder => logBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug).AddConsole().AddEventLog());

            




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.ApplicationServices.GetRequiredService<IInstaller>().InstallService(services, Configuration);
            //app.UseInitializers();
            
            app.UseHealthAllEndpoints();
            app.UseConvey().UseMetricsActiveRequestMiddleware().UseWebSockets();
            
            
            
        }
    }
}
