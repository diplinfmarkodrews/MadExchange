using Convey;
using Convey.Discovery.Consul;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using MadXchange.Connector.Installer;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            IConfiguration configuration = MyWebHostExtensions.GetConfiguration();

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

            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Service"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Provider"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("DataContractAttribute"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Handle"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Descriptor"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //       .Where(c => c.Name.EndsWith("Middleware"))
            //       .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //       .Where(c => c.Name.EndsWith("Command"))
            //       .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Event"))
            //        .AsPublicImplementedInterfaces();
            //services.RegisterAssemblyPublicNonGenericClasses()
            //        .Where(c => c.Name.EndsWith("Query"))
            //        .AsPublicImplementedInterfaces();
            
            //services.AddPolicyRegistry();
            services.AddHttpClientServices(Configuration);
            //services.AddVaultService(Configuration);
            
            var _config = MyWebHostExtensions.GetConfiguration();//used for testaccounts, for dev only!!
            services.AddCacheServices(_config);
            services.AddExchangeAccessServices(_config);
            //services.AddSocketConnectionService();
           // services.AddWebSocketHandler();
            


            

            services.AddHostedService<TimedPollService>();

            //services.AddWebEncoders();
            //services.AddMetrics().AddMetricsEndpoints();

            //
            //services.AddLogging(logBuilder
            //   => MyWebHostExtensions.CreateSerilogLogger(Configuration));
            services.AddSingleton<IServiceId, ServiceId>();
            services.Configure<KestrelServerOptions>(options =>  options.Configure() )
                   // .AddOpenTracing()
                    .AddConvey("connector")
                           
                    //.AddConsul()
                          
                    .AddMongo()
                   
                    .AddMongoRepository<Order, Guid>("orders")
                    //.AddMongoRepository<Position, Guid>("positions")
                    //.AddMongoRepository<Margin, Guid>("margin")
                    //.AddCommandHandlers()
                    //.AddEventHandlers()
                    //.AddQueryHandlers()
                    //.AddInMemoryCommandDispatcher()
                    //.AddInMemoryEventDispatcher()
                    //.AddInMemoryQueryDispatcher()
                    //.AddJaeger()
                    //.AddMetrics()

                    //.AddRabbitMq()
                    //.AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                    //.AddRedis()
                           
                        ;
                           
        }
                        
             
        

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app)//, ILoggerFactory loggerFactory)
        {

            //app.UseDispatcherEndpoints(endpoints => endpoints
            // .Get("", ctx => ctx.Response.WriteAsync("Orders Service"))
            // .Get<GetOrder, OrderDto>("orders/{orderId}")
            // .Post<CreateOrder>("orders",
            //     afterDispatch: (cmd, ctx) => ctx.Response.Created($"orders/{cmd.OrderId}")))
            app.UseSession();
            //.Build();
            //app.UseExceptionHandler(options: new ExceptionHandlerOptions().);
           
           // app.UseWebSockets();
           // app.MapWebSocketManager();

            //.UseInitializers()
            // app.ConfigureEventBus();
            //// app.UseRabbitMq();

            // app.UseMetricsActiveRequestMiddleware();
            // app.UseHealthEndpoint();
            // //app.UseMetricsRequestTrackingMiddleware();
            // app.UseHealthAllEndpoints();
            // app.UsePingEndpoint();

            //app.AddRabbitMq();
            app.UseConvey()
                .UseRouting()
                
                //.UsePingEndpoint()
                //.ConfigureCache(Configuration)
               // .UseJaeger().UseMetrics()
                ;

            //app.UseEndpoints(endpoints =>
            //{
                
            //    //endpoints.MapDefaultControllerRoute();
            //    //endpoints.MapControllers();
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
            

            
             
            
            
            //app.UseConvey();
            
            //app.UseVault();
            ////UseLogging();
            




            //

        }
    }
}