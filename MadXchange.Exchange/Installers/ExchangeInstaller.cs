using MadXchange.Common.Types;
using MadXchange.Exchange.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Installers
{
    /// <summary>
    /// Installs ExchangeDescriptors from Configuration and registers http client factories
    /// </summary>
    public class ExchangeInstaller : IInstaller
    {

        public void InstallService(IServiceCollection services, IConfiguration config)
        {

            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ExchangeInstaller>>();
            logger.LogDebug("Installing ExchangeDescriptors...");
            var descriptors = config.GetSection("ExchangeDescriptors").GetChildren();
            if (descriptors.Count() == 0) return;
            Dictionary<Domain.Models.Exchanges, ExchangeDescriptor> exchangeDictionary = new Dictionary<Domain.Models.Exchanges, ExchangeDescriptor>();
            foreach (var exchange in descriptors) 
            {
                try
                {
                    var exchangeDescriptor = new ExchangeDescriptor();
                    exchange.Bind(exchangeDescriptor);
                    var exchangeEnum = Enum.Parse<Domain.Models.Exchanges>(exchangeDescriptor.Name);
                    exchangeDictionary.Add(exchangeEnum, exchangeDescriptor);

                }
                catch(Exception err) 
                {
                    logger.LogError(err, "error registering exchange descriptor", exchange);
                }
                
            }
            logger.LogDebug("registering exchange dictionary", exchangeDictionary);
            services.AddSingleton(exchangeDictionary);
            
            
            
        }
    }
}
