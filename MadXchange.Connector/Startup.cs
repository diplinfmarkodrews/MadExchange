using Autofac;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
//using Convey.MessageBrokers.RawRabbit;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using MadXchange.Connector.Installer;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Connector.Messages.Events;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceStack.Text;
using System;
using System.Runtime.Serialization;

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
            JsConfig.Init(new Config
            {
                DateHandler = DateHandler.ISO8601,
                AlwaysUseUtc = true,
                TextCase = TextCase.CamelCase,
                ExcludeDefaultValues = true,
                PropertyConvention = PropertyConvention.Lenient,
            });
            JsConfig.AllowRuntimeTypeWithAttributesNamed = new System.Collections.Generic.HashSet<string>
            {
                nameof(DataContractAttribute),
            };
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("exchangesettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("provideraccounts.json", optional: false, reloadOnChange: true);
            builder.AddInMemoryCollection().AddEnvironmentVariables();
            Configuration = builder.Build();
            
            services.AddPolicyRegistry();
            services.AddHttpClientServices(Configuration);
            services.AddVaultService(Configuration);
            services.AddExchangeAccessServices(Configuration);
            services.InstallCacheServices(Configuration);

           

            
            //services.AddWebEncoders();
            services.AddMetrics().AddMetricsEndpoints();
            services.AddOpenTracing();
            //services.AddErrorHandler<ExceptionToResponseMapper>();
            services.AddLogging(logBuilder => logBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug).AddConsole());
            services.AddHostedService<TimedPollService>();
            services.AddSingleton<IServiceId, ServiceId>();
            services.AddConvey("connector")
                    .AddRedis()                    
                    .AddEventHandlers()
                    .AddMetrics()
                    .AddInMemoryEventDispatcher()
                    .AddCommandHandlers()
                    .AddInMemoryCommandDispatcher()
                    .AddQueryHandlers()
                    .AddInMemoryQueryDispatcher()
                    .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                    .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                    .AddJaeger()
                    .AddMongo()
                    .AddMongoRepository<ApiKeySet, Guid>("apikeys")
                    .AddMongoRepository<Order, Guid>("order")
                    .AddInitializer<IMongoDbInitializer>();
                                                                                                 
        }

        // This method gets called by the runtime. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHealthAllEndpoints();
            app.UseConvey().UseMetricsActiveRequestMiddleware().UseWebSockets();
            app.UseMetrics();
            app.UseRouting();
            app.UseExceptionHandler();            
            app.UseJaeger();            
            app.ConfigureEventBus();
            
        }
        


        
        

        
    }
}
