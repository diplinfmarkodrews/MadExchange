using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers.RabbitMQ.Initializers;
using Convey.MessageBrokers.RabbitMQ;
namespace MadXchange.Connector.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
        {
            

            return services;
        }

    }
}
