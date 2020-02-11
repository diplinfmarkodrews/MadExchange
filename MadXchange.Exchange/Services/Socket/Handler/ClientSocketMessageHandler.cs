using MadXchange.Common;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.Socket
{
    public sealed class ClientSocketMessageHandler : WebSocketHandler
    {

        private SocketConnectionStore _connectionStore;
        private Dictionary<int, TaskCompletionSource<InvocationResult>> _waitingRemoteInvocations = new Dictionary<int, TaskCompletionSource<InvocationResult>>();

        public ClientSocketMessageHandler(SocketConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }


        public override async Task OnConnected(WebSocket socket)
        {
            //set state to connected and send subscriptions
            var socketId = socket.GetHashCode();
            var connection = _connectionStore.GetConnection(socketId);
            connection.ConnectionStatus = ConnectionStatus.Connected;            
            await SendSubscriptionsAsync().ConfigureAwait(false);
        }

        private async Task SendSubscriptionsAsync()
        {

            new CancellationTokenSource(1000 * 1).Token.Register(() => { _waitingRemoteInvocations.Remove(invocationDescriptor.Identifier); task.TrySetCanceled(); });
            _waitingRemoteInvocations.Add(invocationDescriptor.Identifier, task);
        }

        public async Task ReceiveAsync(WebSocket socket, SocketMessageDto message)
        {
            if(message.Type == MessageType.Data) 
            {

                return;
            }
            if(message.Type == MessageType.Ctrl)
            {
                if (message.Success) 
                {
                    socket.GetHashCode()
                    _waitingRemoteInvocations
                }
            }

        }

        public async Task SendRequestAsync(Guid accountId, SocketRequestDto objDto, CancellationToken token = default)
                => await SendMessageAsync(_connectionStore.GetSocketById(accountId), objDto, token);
              

        public async Task<T> InvokeClientMethodAsync<T>(int socketId, string methodName, object[] arguments)
        {
            // create the method invocation descriptor.
            InvocationDescriptor invocationDescriptor = new InvocationDescriptor { MethodName = methodName, Arguments = arguments };

            // generate a unique identifier for this invocation.
            invocationDescriptor.Identifier = Guid.NewGuid();

            // add ourselves to the waiting list for return values.
            TaskCompletionSource<InvocationResult> task = new TaskCompletionSource<InvocationResult>();
            // after a timeout of 60 seconds we will cancel the task and remove it from the waiting list.
            //new CancellationTokenSource(1000 * 60).Token.Register(() => { _waitingRemoteInvocations.Remove(invocationDescriptor.Identifier); task.TrySetCanceled(); });
            //_waitingRemoteInvocations.Add(invocationDescriptor.Identifier, task);

            //// send the method invocation to the client.
            //var message = new Message() { MessageType = MessageType.MethodInvocation, Data = JsonConvert.SerializeObject(invocationDescriptor, _jsonSerializerSettings) };
            //await SendMessageAsync(socketId, message).ConfigureAwait(false);

            // wait for the return value elsewhere in the program.
            InvocationResult result = await task.Task;

            // ... we just got an answer.

            // if we have completed successfully:
            if (task.Task.IsCompleted)
            {
                // there was a remote exception so we throw it here.
                if (result.Exception != null)
                    throw new Exception(result.Exception.StackTrace);

                // return the value.

                // support null.
                if (result.Result == null) return default(T);
                // cast anything to T and hope it works.
                return (T)result.Result;
            }

            // if we reach here we got cancelled or alike so throw a timeout exception.
            throw new TimeoutException(); // todo: insert fancy message here.
        }

        public override Task OnDisconnected(WebSocket socket)
        {
            throw new NotImplementedException();
        }
    }
}

//{
//           if (socket.State != WebSocketState.Open)
//               return;
//           await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
//                                                                offset: 0,
//                                                                 count: message.Length),
//                                                           messageType: WebSocketMessageType.Text,
//                                                          endOfMessage: true,
//                                                     cancellationToken: token).ConfigureAwait(false);
//       }
//=> await SendMessageAsync(WebSocketConnectionStore.GetSocketById(socketId).WebSocket, message, token).ConfigureAwait(false);
//protected SocketConnectionStore WebSocketConnectionStore { get; set; }

//public WebSocketHandler(SocketConnectionStore socketConnectionStore)
//    => WebSocketConnectionStore = socketConnectionStore;

//=> await WebSocketConnectionStore.RemoveSocket(id)
//                                 .WebSocket
//                                 .CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
//                                       statusDescription: $"Graceful shut down, connectionId: {id}",
//                                       cancellationToken: CancellationToken.None).ConfigureAwait(false);
