using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MadXchange.Connector.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SocketPipesIoMiddleWare
    {
        private readonly RequestDelegate _next;

        public SocketPipesIoMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
        private async Task ProcessLinesAsync(Socket socket)
        {
            // Console.WriteLine($"[{socket.RemoteEndPoint}]: connected");
            
            // Create a PipeReader over the network stream
            var stream = new NetworkStream(socket);
            var reader = PipeReader.Create(stream);
            var writer = PipeWriter.Create(stream);
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                {
                    // Process the line.
                    ProcessLine(socket, line);
                }

                // Tell the PipeReader how much of the buffer has been consumed.
                reader.AdvanceTo(buffer.Start, buffer.End);
                
                // Stop reading if there's no more data coming.
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Mark the PipeReader as complete.
            await reader.CompleteAsync();

            //Console.WriteLine($"[{socket.RemoteEndPoint}]: disconnected");
        }

        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            // Look for a EOL in the buffer.
            SequencePosition? position = buffer.PositionOf((byte)'\n');

            if (position == null)
            {
                line = default;
                return false;
            }

            // Skip the line + the \n.
            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        private void ProcessLine(Socket socket, in ReadOnlySequence<byte> buffer)
        {
            //if (_accept)
            //{
            //    var message = new StringBuilder();
            //    foreach (var segment in buffer)
            //    {
            //        message.Append(Encoding.UTF8.GetString(segment.Span.ToArray()));
            //    }
            //    MessageReceived(new Record<object>
            //    {
            //        EndPoint = socket.RemoteEndPoint,
            //        Message = message.ToString().Trim('"'),//due to JsonConvert.SerializeObject
            //        Error = string.Empty
            //    });
            //}
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SocketPipesIoMiddleWareExtensions
    {
        public static IApplicationBuilder UseSocketPipesIoMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketPipesIoMiddleWare>();
        }
    }
}
