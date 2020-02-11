using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.RabbitMQ;

//using Convey.MessageBrokers.RawRabbit;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using MadXchange.Connector.Installer;
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

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("exchangesettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("testaccounts.json", optional: false, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();


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
                nameof(RuntimeSerializableAttribute)
            };
            //JsConfig.AllowRuntimeType = type => type == typeof(WebResponseDto);
            services.AddPolicyRegistry();
            services.AddHttpClientServices(Configuration);
            services.AddVaultService(Configuration);

            services.AddCacheServices(Configuration);

            services.AddSocketConnectionService();
            services.AddWebSocketHandler();
            services.AddExchangeAccessServices(Configuration);
            

            //services.AddWebEncoders();
            services.AddMetrics().AddMetricsEndpoints();

            services.AddOpenTracing();
            services.AddLogging(logBuilder => logBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug).AddConsole());
            
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

            services.AddHostedService<TimedPollService>();
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler();
            // loggerFactory.AddSerilog();
            app.UseWebSockets();
            app.UseMetrics();
            app.UseRouting();
            app.MapWebSocketManager();
            app.UseConvey();
            app.ConfigureEventBus();
            app.UseMetricsActiveRequestMiddleware();
            app.UseHealthAllEndpoints();

            app.UseJaeger();
            app.UsePingEndpoint();
           
        }
    }
}