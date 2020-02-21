using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Testing;

namespace MadXchange.Connector.AppHost
{
    using Funq;
    using ServiceStack;
    using ServiceStack.Admin;

    using ServiceStack.Data;
    using ServiceStack.OrmLite;
  
    

    namespace MadXchange.Connector.SSHost
    {
        public class AppHost : AppHostBase
        {
            public static Guid AppId= Guid.NewGuid();
            /// <summary>
            /// Default constructor.
            /// Base constructor requires a name and assembly to locate web service classes. 
            /// </summary>
            public AppHost() : base(AppId+"-Connector", typeof(Program).Assembly) 
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
}
