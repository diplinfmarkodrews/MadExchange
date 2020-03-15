using System;
using System.Collections.Generic;
using Funq;
using ServiceStack;



namespace MadXchange.Connector.AppHost
{
    public sealed class AppHost : AppHostHttpListenerSmartPoolBase
    {
        public static Guid AppId = Guid.NewGuid();
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost() : base($"Connector#{AppId}", typeof(Program).Assembly, typeof(Startup).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples



        }
    }
}

