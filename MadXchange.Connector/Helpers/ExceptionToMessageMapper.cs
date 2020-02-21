
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

    internal interface IExceptionToMessageMapper
    {
    }
}