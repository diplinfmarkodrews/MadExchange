using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Tracing.Jaeger;
using MadXchange.ClientExecution.Installer;
using MadXchange.ClientExecution.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ClientTradeExecutionService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton<IServiceId, ServiceId>();
            services.AddOpenTracing();
            services.AddConvey("ClientExecution")
                    .AddCommandHandlers()
                    .AddEventHandlers()
                    .AddQueryHandlers()
                    .AddInMemoryCommandDispatcher()
                    .AddInMemoryEventDispatcher()
                    .AddInMemoryQueryDispatcher()
                    .AddJaeger()
                    .AddMetrics()
                    .AddRabbitMq();
            services.AddTransient<ConnectionManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseElmCapture();
            app.UseConvey();
            app.ConfigureEventBus();
            var conManager = app.ApplicationServices.GetRequiredService<ConnectionManager>();
            conManager.SetLeverage(Guid.Parse("495686fe-c034-426c-94dd-2f1f5bf00389"), "BTCUSD", 2.0m);
            
        }
    }
}
