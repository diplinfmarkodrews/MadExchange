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
            services.AddConvey("connector");
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddWebEncoders();
            services.AddLogging(logBuilder => logBuilder.SetMinimumLevel(LogLevel.Debug).AddConsole().AddEventLog().AddSerilog());

            Configuration = new ConfigurationBuilder().AddJsonFile($"{Directory.GetCurrentDirectory()}/exchangesettings.json").AddJsonFile($"{Directory.GetCurrentDirectory()}/appsettings.json", true, true).Build();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseConvey().UseMetricsActiveRequestMiddleware().UseWebSockets();
            app.UseRouting();
            
            
        }
    }
}
