using Autofac;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Connector.Messages.Events;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Installers;
using MadXchange.Connector.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

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
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("exchangesettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("provideraccounts.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();


            services.AddHttpClientServices(Configuration);
            services.AddVaultService(Configuration);
            services.InstallExchangeAccessServices(Configuration);
            services.InstallCacheServices(Configuration);

            services.AddPolicyRegistry();
          
            
            //services.AddWebEncoders();
            services.AddMetrics().AddMetricsEndpoints();
            services.AddOpenTracing();
            //services.AddVault();
            services.AddLogging(logBuilder => logBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug).AddConsole());
            services.AddHostedService<TimedPollService>();
            

            services.AddConvey("connector")
                                           .AddEventHandlers()
                                           .AddInMemoryEventDispatcher()
                                           .AddCommandHandlers()
                                           .AddInMemoryCommandDispatcher()
                                           .AddQueryHandlers()
                                           .AddInMemoryQueryDispatcher()
                                           .AddRabbitMq("connect")
                                           .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                                           .AddJaeger()                                           
                                           .AddMongo()
                                           .AddMongoRepository<ApiKeySet, Guid>("apikeys")
                                           .AddMongoRepository<Order, Guid>("order")
                                           .AddInitializer<IMongoDbInitializer>()
                                           
                                           ;
            

        }

        // This method gets called by the runtime. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         
            //app.UseInitializers();  
            
            app.UseHealthAllEndpoints();
            app.UseConvey().UseMetricsActiveRequestMiddleware().UseWebSockets();
            app.UseRabbitMq()
                    .SubscribeCommand<CreateOrder>()
                    .SubscribeEvent<OrderRejectedEvent>()
                    .SubscribeEvent<OrderPlacedEvent>()
                    .SubscribeCommand<UpdateOrder>()
                    .SubscribeEvent<OrderUpdatedEvent>()
                    .SubscribeEvent<OrderUpdateRejectedEvent>()
                    .SubscribeCommand<CancelOrder>()
                    .SubscribeEvent<CancelOrderEvent>()
                    .SubscribeEvent<CancelOrderRejectedEvent>()
                    .SubscribeCommand<SetLeverage>()
                    .SubscribeEvent<LeverageSetEvent>()
                    .SubscribeEvent<LeverageSetRejectedEvent>();
            app.UseJaeger();
            

        }
        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
           // var eventBus = app.ApplicationServices.GetRequiredService<IBusSubscriber>();
            //eventBus.Subscribe<IRabbitMq>();
            //eventBus.Subscribe<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

        }


        
        

        
    }
}
