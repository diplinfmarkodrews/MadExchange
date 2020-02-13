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
    public abstract class WebSocketHandler
    {
        //called when socketrequest accepted => send subscriptions
        public abstract Task OnConnected(WebSocket socket);
        //disconnected by the exchange => try to reconnect
        public abstract Task OnDisconnected(WebSocket socket);

        

        public async Task CloseAsync(WebSocket socket, WebSocketCloseStatus closeStatus)
        {            
            await socket.CloseAsync(closeStatus: closeStatus,
                              statusDescription: $"Socket was closed! new status {closeStatus}",
                              cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }


        protected async Task SendRequestAsync(WebSocketConnection connection, SocketRequest message, CancellationToken token)
        {
            if (connection.Socket.State != WebSocketState.Open)
                return;
            
            var serializedMessage = message.RequestDto.SerializeAndFormat();
            var encodedMessage = Encoding.UTF8.GetBytes(serializedMessage);
            await connection.Socket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage,
                                                                            offset: 0,
                                                                             count: encodedMessage.Length),
                                         messageType: WebSocketMessageType.Text,
                                        endOfMessage: true,
                                   cancellationToken: token).ConfigureAwait(false);
        }

    }
}

