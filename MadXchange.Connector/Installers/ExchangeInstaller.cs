using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Services;
using MadXchange.Exchange.Services.HttpRequests;
using MadXchange.Exchange.Services.RequestExecution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Installers
{

    /// <summary>
    /// Installs ExchangeDescriptors from Configuration and registers http client factories
    /// </summary>
    public static class ExchangeInstaller 
    {
        
        public static void InstallExchangeDescriptorDictionary(this IServiceCollection services, IConfiguration config)
        {
                      
            var descriptors = config.GetSection("ExchangeDescriptors").GetChildren();
            if (descriptors.Count() == 0) return;
            Dictionary<Domain.Models.Exchanges, ExchangeDescriptor> exchangeDictionary = new Dictionary<Domain.Models.Exchanges, ExchangeDescriptor>();
            foreach (var exchange in descriptors) 
            {
                try
                {
                    var exchangeDescriptor = new ExchangeDescriptor();
                    exchangeDescriptor.Name = exchange.GetValue<string>("Name");
                    exchangeDescriptor.BaseUrl = exchange.GetValue<string>("BaseUrl");
                    exchangeDescriptor.SocketUrl = exchange.GetValue<string>("SocketUrl");
                    var exchangeEnum = Enum.Parse<Domain.Models.Exchanges>(exchangeDescriptor.Name);
                    exchangeDescriptor.Id = (int)exchangeEnum;
                    var getRoutes = exchange.GetSection("Routes:GET");
                    var postRoutes = exchange.GetSection("Routes:POST");
                    readRoutes(ref exchangeDescriptor, getRoutes);
                    readRoutes(ref exchangeDescriptor, postRoutes);
                    exchangeDictionary.Add(exchangeEnum, exchangeDescriptor);

                }
                catch { }          
                
            }
            //logger.LogDebug("registering exchange dictionary", exchangeDictionary);
            services.AddSingleton(exchangeDictionary);
            services.AddTransient<IExchangeDescriptorService, ExchangeDescriptorService>();
            services.AddTransient<IRequestAccessService, RequestAccessService>();
            services.AddTransient<IRestRequestExecutionService, RestRequestExecutionService>();
            services.AddTransient<IRestRequestService, RestRequestService>();
            services.AddTransient<IInstrumentRequestService, InstrumentRequestService>();
            services.AddTransient<IOrderRequestService, OrderRequestService>();
            services.AddTransient<IPositionRequestService, PositionRequestService>();
            services.AddTransient<IMarginRequestService, MarginRequestService>();
            
        }

        private static void readRoutes(ref ExchangeDescriptor descriptor, IConfigurationSection route)
        {
            var routes = route;            
            var iSection = routes.GetSection("Instrument");
            descriptor.RouteGetInstrument = ReadEndPoint<Instrument>(iSection);            
        }

        private static Types.EndPoint<T> ReadEndPoint<T>(IConfigurationSection cSection)
        {
            var endP = new Types.EndPoint<T>();
            endP.Url = cSection.GetSection("url").Value;
            var parameters = cSection.GetSection("parameter").GetChildren();
            var parameterCount = parameters.Count();
            if (parameterCount > 0)
            {
                endP.Parameter = new Types.Parameter[parameterCount];
                for (int i = 0; i < parameterCount; i++)
                {
                    endP.Parameter[i] = new Types.Parameter();
                    endP.Parameter[i].IsRequired = bool.Parse(parameters.ElementAt(i).GetSection("required").Value);
                    var name = parameters.ElementAt(i).Key;
                    var domainName = parameters.ElementAt(i).GetValue<string>("domain");
                    if (!string.IsNullOrEmpty(domainName))
                    {
                        endP.Parameter[i].Param = new RestSharp.NameValuePair(name, domainName);
                    }
                    else
                    {
                        endP.Parameter[i].Param = new RestSharp.NameValuePair(name, name);
                    }
                }
            }
            return endP;
        }
    }
}
