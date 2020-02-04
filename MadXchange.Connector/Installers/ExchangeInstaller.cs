using MadXchange.Connector.Installer.PluginLoader;
using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Infrastructure.Repositories;
using MadXchange.Exchange.Services.HttpRequests;
using MadXchange.Exchange.Services.RequestExecution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MadXchange.Connector.Installer
{

    /// <summary>
    /// Installs ExchangeDescriptors from configuration. 
    /// </summary>
    public static class ExchangeInstaller 
    {
        
        public static IServiceCollection AddExchangeAccessServices(this IServiceCollection services, IConfiguration config)
        {

            ConfigExchangeDescriptors.SetConfig(config);
            services.AddSingleton<IExchangeDescriptorService, ExchangeDescriptorService>();
            services.AddSingleton<IApiKeySetRepository, ApiKeySetRepository>();
            services.AddSingleton<IRequestAccessService, RequestAccessService>();
            services.AddSingleton<ISignRequests, SignRequestService>();
            services.AddSingleton<IRestRequestExecutionService, RestRequestExecutionService>();
            services.AddSingleton<IRestRequestService, RestRequestService>();
            services.AddSingleton<IInstrumentRequestService, InstrumentRequestService>();
            services.AddSingleton<IOrderRequestService, OrderRequestService>();
            services.AddSingleton<IPositionRequestService, PositionRequestService>();
            services.AddSingleton<IMarginRequestService, MarginRequestService>();
            services.AddSingleton<IExchangeRequestService, ExchangeCommandExecutionService>();
            return services;
        }
     

    }
}
