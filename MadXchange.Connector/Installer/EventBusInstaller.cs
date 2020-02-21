
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Connector.Messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MadXchange.Connector.Installer
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusServices(this IServiceCollection services, IConfiguration config)
        {
             
            return services;
        }

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
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

            return app;
        }
    }
}