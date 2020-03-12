using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using MadXchange.Exchange.Messages.Commands.AccountManager;
using MadXchange.Exchange.Messages.Commands.OrderService;
using Microsoft.AspNetCore.Builder;

namespace MadXchange.Connector.Installer
{
    public static class EventBusInstaller
    {
        

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            app.UseRabbitMq()                
                .SubscribeCommand<CreateOrder>()
                .SubscribeCommand<UpdateOrder>()
                .SubscribeCommand<CancelOrder>()
                .SubscribeCommand<SetLeverage>()
                .SubscribeCommand<CancelCommand>()
                .SubscribeCommand<AddAccount>()
                .SubscribeCommand<RemoveAccount>()
                .SubscribeCommand<UpdateAccount>();
            return app;
        }
    }
}