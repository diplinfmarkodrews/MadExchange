using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Domain.Models
{
    class RequestInvocation : IDisposable
    {
        public string RequestKey { get; }
        public SocketRequest Request { get; set; }
        public TaskCompletionSource<SocketInvocationResult> RequestCompletionSource { get; set; }
        public RequestInvocation(string requestKey, SocketRequest request, TaskCompletionSource<SocketInvocationResult> task)
        {
            RequestKey = requestKey;
            Request = request;
            RequestCompletionSource = task;
            //CancellationTokenSource cancellationSource
        }
        public void Dispose()
        {
            ((IDisposable)Request).Dispose();
        }
    }
}
