using Convey.HTTP;
using MadXchange.Connector.Configuration;
using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.HttpRequests;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.Socket;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Services.XchangeDescriptor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModernHttpClient;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MadXchange.Connector.Installer
{
    /// <summary>
    /// Installs ExchangeDescriptors from configuration.
    /// </summary>
    public static class ExchangeInstaller
    {
        //more or less for debug only, keypairs will be provided by vault infrastructure
        private static Dictionary<Guid, ApiKeySet> ReadProviderAccounts(IConfiguration config)
        {
            Dictionary<Guid, ApiKeySet> result = new Dictionary<Guid, ApiKeySet>();
            var keys = config.GetSection("ExchangeKeys").GetChildren();
            foreach (var key in keys)
            {
                var pair = new ApikeyPair();
                key.Bind(pair);
                var kSet = new ApiKeySet(Guid.Parse(pair.Id), Enum.Parse<Xchange>(pair.Exchange), pair.Key, pair.Secret);
                result.Add(kSet.Id, kSet);
            }
            return result;
        }

        public static IServiceCollection AddExchangeAccessServices(this IServiceCollection services, IConfiguration config)
        {
            var userKeys = ReadProviderAccounts(config);
            services.AddSingleton<IXchangeDescriptorConfiguration, XchangeDescriptorConfig>(c => new XchangeDescriptorConfig(config));
            services.AddSingleton<IXchangeDescriptorService, ExchangeDescriptorService>();
            services.AddSingleton<IApiKeySetStore, ApiKeySetStore>(c => new ApiKeySetStore(userKeys));
            services.AddSingleton<IRequestAccessService, RequestAccessService>();
            services.AddSingleton<ISignRequests, SignRequestService>();
            services.AddSingleton<IRequestExecutionService, RequestExecutionService>();
            services.AddSingleton<IRestRequestService, RestRequestService>();
            services.AddSingleton<IInstrumentRequestService, InstrumentRequestService>();
            services.AddSingleton<IPositionRequestService, PositionRequestService>();
            services.AddSingleton<IMarginRequestService, MarginRequestService>();
            services.AddSingleton<IOrderRequestService, OrderRequestService>();
            services.AddSingleton<IXchangeCommands, CommandExecutionService>();
            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            bool legacy = configuration.GetValue<bool>("LegacyHttpMsgHandler");            
            services.AddHttpClient<IHttpClient>("X-CHANGE", cfg =>
            {
                if(legacy)
                    JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler(throwOnCaptiveNetwork: true, customSSLVerification: false);

                JsonHttpClient.DefaultUserAgent = "MadMexIO";
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
            
            

            //add http client services
            //services.AddHttpClient("GrantClient")
            //       .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            //        ;

            return services;
        }

        
    }
}