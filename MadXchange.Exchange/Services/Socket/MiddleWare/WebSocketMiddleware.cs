using MadXchange.Exchange.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;
using System.IO;
using System.Text;

namespace MadXchange.Exchange.Services.Socket
{
    /// <summary>
    /// incoming messages are handled and first deserialization
    /// </summary>
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private ClientSocketMessageHandler _webSocketHandler { get; set; }
        
        public WebSocketMiddleware(RequestDelegate next, ClientSocketMessageHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
            await _webSocketHandler.OnConnected(socket).ConfigureAwait(false);

            await Receive(socket, async (result, serializedMessage) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {                    
                    var message = serializedMessage.GetDto<SocketMessage>();
                    await _webSocketHandler.ReceiveAsync(socket, message).ConfigureAwait(false);
                    return;
                }
                else if (result.MessageType == WebSocketMessageType.Close && socket.CloseStatus != WebSocketCloseStatus.NormalClosure)
                {
                    try
                    {
                        await _webSocketHandler.OnDisconnected(socket);
                    }
                    catch (WebSocketException)
                    {
                        throw; //let's not swallow any exception for now
                    }

                    return;
                }
            }).ConfigureAwait(false);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024*4]);
                string message = null;
                WebSocketReceiveResult result = null;
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await socket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            message = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                    handleMessage(result, message);
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        socket.Abort();
                    }
                }
            }

            await _webSocketHandler.OnDisconnected(socket);
        }
    }
}