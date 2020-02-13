using MadXchange.Common;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Exceptions;
using MadXchange.Exchange.Helpers;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.Socket
{
    /// <summary>
    /// Handles incoming requests of websocketconnections.
    /// Implements a very basic data layer, passing typed messages to the appropriate websocketconnection
    /// </summary>
    public sealed class ClientSocketMessageHandler : WebSocketHandler
    {
        
        private SocketConnectionStore _connectionStore;        

        public ClientSocketMessageHandler(SocketConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public override async Task OnConnected(WebSocket socket)
        {
            //set state to connected and send subscriptions            
            var connection = _connectionStore.GetConnection(socket);
            var startInitRequest = connection.InitOnConnected();
            await SendRequestAsync(connection.Id, startInitRequest).ConfigureAwait(false);           
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketConnection = _connectionStore.GetConnection(socket.GetID());            
            socketConnection.OnDisconnected();
            await socketConnection.StartConnectionAsync().ConfigureAwait(false);
        }

        [MethodImpl(methodImplOptions:MethodImplOptions.AggressiveInlining)]
        public Task ReceiveAsync(WebSocket socket, ISocketMessage message)
        {
            switch (message.MessageType) 
            {
                case MessageType.Data:                    
                    return Task.FromResult(_connectionStore.GetConnection(socket).OnData(message as SocketMessage).ConfigureAwait(false));
                case MessageType.Ctrl:

                    return Task.FromResult(HandleResponseMessage(socket, message as SocketResponse));
                default:
                    throw new InvalidOperationException($"Invalid socketmessagetype\n{message.Dump()}");
            }
           

        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private Task HandleResponseMessage(WebSocket socket, SocketResponse message)  
        {            
            var connection = _connectionStore.GetConnection(socket);
            var socketRequest = connection.OnCtrlMessage(message);
            if (socketRequest != default)             
                return Task.FromResult(SendRequestAsync(connection.Id, socketRequest));
                        
            return Task.CompletedTask;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public async Task SendRequestAsync(Guid accountId, SocketRequest objDto, CancellationToken token = default)
                => await SendRequestAsync(_connectionStore.GetConnection(accountId), objDto, token).ConfigureAwait(false);



       
    }
}

