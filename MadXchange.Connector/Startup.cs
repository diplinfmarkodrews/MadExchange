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
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            Configuration = MyWebHostExtensions.GetConfiguration();

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
            //services.AddHttpClientServices(Configuration);
            //services.AddVaultService(Configuration);
            //services.AddCacheServices(Configuration);

            //services.AddSocketConnectionService();
            //services.AddWebSocketHandler();
            //services.AddExchangeAccessServices(Configuration);


            //services.AddWebEncoders();
            // services.AddMetrics().AddMetricsEndpoints();

            services.AddOpenTracingCoreServices();
            services.AddLogging(logBuilder
                => MyWebHostExtensions.CreateSerilogLogger(Configuration));

            services.AddSingleton<IServiceId, ServiceId>();
            services.AddConvey("connector")
                    .AddRedis()



                    .AddMetrics()
                    .AddEventHandlers()
                    .AddInMemoryEventDispatcher()
                    .AddCommandHandlers()
                    .AddInMemoryCommandDispatcher()
                    .AddQueryHandlers()
                    .AddInMemoryQueryDispatcher()


                    .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                    .AddJaeger()
                    .AddMongo()
                    .AddRabbitMq()//plugins: p => p.AddJaegerRabbitMqPlugin())
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
            app.ConfigureEventBus();
           
            app.UseConvey();
            app.UseRabbitMq();
            // loggerFactory.AddSerilog();
            //app.UseWebSockets();
            app.UseMetrics();
           
            app.UseJaeger();
           // app.MapWebSocketManager();
            
            app.UseRouting();
            
            //app.UseMetricsAllMiddleware();
            //app.UseHealthAllEndpoints();
            //app.UseMetricsAllEndpoints();
         
            app.UsePingEndpoint();
            //app.UseEndpoints()endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //    endpoints.MapControllers();
            //    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
            //    {
            //        Predicate = _ => true,
            //        //ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //    });
            //    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            //    {
            //        Predicate = r => r.Name.Contains("self")
            //    });
            //});

        }
    }
}