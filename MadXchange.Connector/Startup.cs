using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Tracing.Jaeger;
using MadXchange.Connector.Installer;
using MadXchange.Connector.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using ServiceStack.Text;
using System;
using System.Runtime.Serialization;

namespace MadXchange.Connector
{
    public class Startup //: ModularStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private static IConfiguration Configuration = MyWebHostExtensions.GetConfiguration();
        //public Startup(IConfiguration configuration)
        //    : base(configuration, typeof(Startup).Assembly, typeof(Program).Assembly, typeof(ExchangeInstaller).Assembly, typeof(WebSocketInstaller).Assembly) { }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
            IConfiguration configuration = MyWebHostExtensions.GetConfiguration();
            services.AddLogging(builder => builder.AddSerilog(Log.Logger));
            services.AddElm();
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
                nameof(DataMemberAttribute),
                nameof(RuntimeSerializableAttribute)
            };
            //JsConfig.AllowRuntimeType = type => type == typeof(WebResponseDto);

            
            services.AddPolicyRegistry();
            services.AddHttpClientServices(Configuration);
            //services.AddVaultService(Configuration);
            
            var _config = MyWebHostExtensions.GetConfiguration();//used for testaccounts, for dev only!!
            services.AddCacheServices(_config);
            services.AddExchangeAccessServices(_config);
            services.AddSocketConnectionService();          
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            });

          
            /////
            services.AddWebEncoders();
            services.AddMetrics()
                    .AddMetricsEndpoints()
                    .AddMetricsTrackingMiddleware();
                    //.AddMetricsReportingHostedService((exc, context) => Log.Logger.Error("exception: ", context.Exception));

            //
            //services.AddLogging(logBuilder
            //   => MyWebHostExtensions.CreateSerilogLogger(Configuration));
            services.AddSingleton<IServiceId, ServiceId>();

            services.AddOpenTracing();
            services.AddConvey("connector")

                    //.AddConsul()

                    //.AddMongo()

                    //.AddMongoRepository<Order, Guid>("orders");
                    //.AddMongoRepository<Position, Guid>("positions")
                    //.AddMongoRepository<Margin, Guid>("margin")
                    .AddCommandHandlers()
                    .AddEventHandlers()
                    .AddQueryHandlers()

                    .AddInMemoryEventDispatcher()
                    .AddInMemoryQueryDispatcher()
                    .AddJaeger()
                    .AddMetrics()

                    .AddRabbitMq();
                    //.AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                    //.AddRedis()
                           
                        
            services.AddHostedService<TimedPollService>();
        }




        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app)//, ILoggerFactory loggerFactory)
        {
           
            ILoggerFactory loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            //app.UseDispatcherEndpoints(endpoints => endpoints
            // .Get("", ctx => ctx.Response.WriteAsync("Orders Service"))
            // .Get<GetOrder, OrderDto>("orders/{orderId}")
            // .Post<CreateOrder>("orders",
            //     afterDispatch: (cmd, ctx) => ctx.Response.Created($"orders/{cmd.OrderId}")))
            //app.UseSession();
            //.Build();
            //app.UseExceptionHandler(options: new ExceptionHandlerOptions());
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            // Exception handling logging below
            app.UseElmCapture();
            //app.UseElmPage();
            app.UseDeveloperExceptionPage();


            //app.UseExceptionHandler("/Error");
            //.UseInitializers()
            // app.ConfigureEventBus();
            // app.UseRabbitMq();

            app.UseMetricsActiveRequestMiddleware();
            // app.UseHealthEndpoint();
            app.UseMetricsRequestTrackingMiddleware();
            // app.UseHealthAllEndpoints();
            // app.UsePingEndpoint();
            //app.ConfigureCache();
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
           
            app.UseWebSockets(webSocketOptions);
           
           // app.UseConvey();
            app.UseRouting();
            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UsePingEndpoint();
            app.UseJaeger();
            app.StartSocketConnections();
        }
    }
}