
using MadXchange.Connector.Configuration;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Services.HttpRequests;
using MadXchange.Exchange.Services.HttpRequests.RequestExecution;
using MadXchange.Exchange.Services.Utils;
using MadXchange.Exchange.Services.XchangeDescriptor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModernHttpClient;
using Serilog;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

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

        public static IConfiguration XchangeConfig()
        {
            var confBuilder = new ConfigurationBuilder();
            var exchangeFiles = Path.Combine($"{Directory.GetCurrentDirectory()}/XchangeConfigs/");
            var exchangeConfigFiles = Directory.GetFiles(exchangeFiles);
            foreach (var f in exchangeConfigFiles)
                confBuilder.AddJsonFile(f, optional: true, reloadOnChange: true);
            var config = confBuilder.Build();
            return config;
        }

        public static IServiceCollection AddExchangeAccessServices(this IServiceCollection services, IConfiguration config)
        {
            var exchangeConfigs = XchangeConfig();
            var userKeys = ReadProviderAccounts(config);
            services.AddSingleton<IXchangeDescriptorConfiguration, XchangeDescriptorConfig>(c => new XchangeDescriptorConfig(exchangeConfigs));
            services.AddTransient<IXchangeDescriptorService, ExchangeDescriptorService>();
            services.AddTransient<IApiKeySetStore, ApiKeySetStore>(c => new ApiKeySetStore(userKeys));
            services.AddTransient<IRequestAccessService, RequestAccessService>();
            services.AddTransient<ISignRequestsService, SignRequestService>();
            services.AddTransient<IRequestExecutionService, RequestExecutionService>();
            services.AddTransient<IRestRequestService, RestRequestService>();
            services.AddTransient<IInstrumentRequestService, InstrumentRequestService>();
            services.AddTransient<IPositionRequestService, PositionRequestService>();
            //services.AddSingleton<IMarginRequestService, MarginRequestService>();
            //services.AddSingleton<IOrderRequestService, OrderRequestService>();
            //services.AddSingleton<IXchangeCommands, CommandExecutionService>();
            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            //bool legacy = configuration.GetValue<bool>("LegacyHttpMsgHandler");            
            services.AddHttpClient("XCHANGE", cfg =>
            {                
                JsonHttpClient.GlobalHttpMessageHandlerFactory = () => new NativeMessageHandler(throwOnCaptiveNetwork: true, customSSLVerification: false);
                JsonHttpClient.DefaultUserAgent = "MadMexIO";
                JsonHttpClient.log = LogManager.GetLogger(typeof(JsonHttpClient));              
            
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
                
          
            //add http client services
            //services.AddHttpClient("GrantClient")
            //       .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            //        ;

            return services;
        }

        
    }
}