using System;

namespace MadXchange.Connector
{
    public class ErrorData
    {
        public string Method { get; }
        public Exception Exception { get; }

        public ErrorData(string method, Exception exp)
        {
            Method = method;
            Exception = exp;
        }
    }
}
