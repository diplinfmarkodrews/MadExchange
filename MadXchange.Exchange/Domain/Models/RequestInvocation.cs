using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Domain.Models
{
    class RequestInvocation : IDisposable
    {
        public Guid RequestKey { get; }
        public SocketRequest Request { get; set; }
        public TaskCompletionSource<SocketInvocationResult> RequestCompletionSource { get; set; }
        public RequestInvocation(Guid requestKey, SocketRequest request, TaskCompletionSource<SocketInvocationResult> task)
        {
            RequestKey = requestKey;
            Request = request;
            RequestCompletionSource = task;
        }
        public void Dispose()
        {
           
            ((IDisposable)Request).Dispose();
        }
    }
}
