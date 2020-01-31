using Convey.MessageBrokers.RabbitMQ;
using System;

namespace MadXchange.Connector
{
    internal class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        {
            return exception;
        }
    }
}