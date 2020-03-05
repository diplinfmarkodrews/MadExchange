using MadXchange.Common;
using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Models
{
    internal class SocketInvocationResult : InvocationResult
    {

        internal SocketMethod SocketMethod { get; set; }
        internal Guid RequestKey { get; set; }
        internal SocketInvocationResult() : base() 
        {
        
        }
    }
}
