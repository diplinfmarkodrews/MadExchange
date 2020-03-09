using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using MadXchange.Connector.Messages.Events;
using MadXchange.Exchange.Messages.Events;
using MadXchange.Exchange.Messages.Events.CancelCommand;
using Microsoft.AspNetCore.Builder;

namespace MadXchange.ClientExecution.Installer
{
    public static class EventBusInstaller
    {

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            app.UseRabbitMq()
                .SubscribeEvent<OrderRejectedEvent>()
                .SubscribeEvent<OrderPlacedEvent>()
                .SubscribeEvent<OrderUpdatedEvent>()
                .SubscribeEvent<OrderUpdateRejectedEvent>()                
                .SubscribeEvent<CancelOrderEvent>()
                .SubscribeEvent<CancelOrderRejectedEvent>()                
                .SubscribeEvent<LeverageSetEvent>()
                .SubscribeEvent<LeverageSetRejectedEvent>()
                .SubscribeEvent<CancelCommandEvent>()
                .SubscribeEvent<CancelCommandRejectedEvent>();
            return app;
        }
    }
}
