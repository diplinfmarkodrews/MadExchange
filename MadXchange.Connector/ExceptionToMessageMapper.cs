using Convey.MessageBrokers.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadXchange.Connector
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        {
            switch (exception)
            {
                // do simple pattern matching
            }

            return null;
        }
    }
}
