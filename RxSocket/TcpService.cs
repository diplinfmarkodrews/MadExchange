using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO.Pipelines;

using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MadXchange.Exchange.Contracts;
//using MadXchange.Exchange.Domain.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack;

namespace MadXchange.Connector
{
    public interface ISocketService
    {
        IObservable<Socket> Accepted { get; }
        IObservable<EndPoint> Disconnected { get; }
        IObservable<ErrorData> Error { get; }
        IObservable<Record<object>> Reciever { get; }
        IObservable<Record<object>> Sender { get; }
        Task StartAsync();
        void Stop();
        Task SendAsync<T>(T message, Action<Record<T>> errorMessageCallback = null);
    }
    public interface ISocketClientService 
    {
        Task ReceiveAsync(Socket socket, int maxBufferSize);
    }
    

    public sealed class TcpService : ISocketService
    {
        private bool _accept { get; set; }
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private readonly ISubject<Socket> _accepted = new Subject<Socket>();
        private readonly ISubject<EndPoint> _disconnected = new Subject<EndPoint>();
        private readonly ISubject<ErrorData> _error = new Subject<ErrorData>();
        private readonly ISubject<Record<object>> _sender = new Subject<Record<object>>();
        private readonly ConcurrentDictionary<int, Socket> _connections = new ConcurrentDictionary<int, Socket>();
        private readonly Socket _listenerSocket;
        private readonly int _backlog;
        private readonly int _bufferSize;
        private readonly int _retryMax;
        private readonly ILogger _logger;

        public TcpService(Microsoft.Extensions.Options.IOptions<ServerConfig> serverConfig, ILogger<TcpService> logger)
            : this(serverConfig.Value.IpAddress,
                   serverConfig.Value.Port,
                   serverConfig.Value.Backlog,
                   serverConfig.Value.BufferSize)
        {
            _logger = logger;
            _retryMax = serverConfig.Value.Retry;
        }

        public TcpService(ServerConfig serverConfig)
            : this(serverConfig.IpAddress,
                   serverConfig.Port,
                   serverConfig.Backlog,
                   serverConfig.BufferSize)
        {
            
            _retryMax = serverConfig.Retry;
        }

        

        public TcpService(string IP, int port, int backlog, int bufferSize)
        {
            
            var address = IPAddress.Parse(IP);
            _backlog = backlog;
            _bufferSize = bufferSize;
            _listenerSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            var endPoint = new IPEndPoint(address, port);
            try
            {
                _listenerSocket.Bind(endPoint);
            }
            catch
            {
                var result = Utility.Retry(_retryMax,
                     () => Task.Factory.StartNew(() => _listenerSocket.Bind(endPoint))
                 );
                if (!result)
                {
                    throw;
                }
            }
        }

        private readonly Action<int> ShowTotalConnections = (totals) =>
        {
            //_logger.LogDebug($"Waiting for client... {totals} connected at the moment.");
        };

        private readonly Action<EndPoint> SocketDisposed = (ep) =>
        {
            //_logger.LogDebug($"[{ep}]: client socket disposed.");
        };

        private event Action<Record<object>> MessageReceived = _ => 
        {
            var msgObj = _.Message.GetResponseDto<SocketMessageDto>();
           
        };

        public IObservable<Socket> Accepted => _accepted.AsObservable();

        public IObservable<EndPoint> Disconnected => _disconnected.AsObservable();

        public IObservable<ErrorData> Error => _error.AsObservable();

        //
        public IObservable<Record<object>> Reciever 
            => Observable.FromEvent<Record<object>>(a => MessageReceived += a, a => MessageReceived -= a);

        public IObservable<Record<object>> Sender => _sender.AsObservable();

        async Task ISocketService.StartAsync()
        {
            _listenerSocket.Listen(_backlog);
            _accept = true;
            await Task.Run(Listen);
        }

        private async Task Listen()
        {
            if (_listenerSocket != null && _accept)
            {
                _accepted.OnNext(_listenerSocket);
                // Continue listening.               
                while (true)
                {
                    var socket = await _listenerSocket.AcceptAsync().ConfigureAwait(false);
                    _ = ProcessLinesAsync(socket, _bufferSize);
                }
            }
        }

        private async Task ProcessLinesAsync(Socket socket, int bufferSize)
        {
            _logger.LogDebug($"[{socket.RemoteEndPoint}]: connected");
            _connections.AddOrUpdate(socket.GetHashCode(), socket, (key, oldValue) => socket);
            ShowTotalConnections(_connections.Count);

            var pipe = new Pipe();
            var writing = FillPipeAsync(socket, pipe.Writer, bufferSize);
            var reading = ReadPipeAsync(socket, pipe.Reader);

            await Task.WhenAll(reading, writing).ConfigureAwait(false);
                       
            _logger.LogDebug($"[{socket.RemoteEndPoint}]: disconnected");
            _connections.TryRemove(socket.GetHashCode(), out var socketClient);
            ShowTotalConnections(_connections.Count);
        }

        //public async Task ReceiveMessageAsync(Socket socket, PipeWriter writer, int minimumBufferSize)
        //     => await FillPipeAsync(socket, writer, minimumBufferSize);

        private async Task FillPipeAsync(Socket socket, PipeWriter writer, int minimumBufferSize)
        {
            while (true)
            {
                try
                {
                    // Request a minimum of 1024 bytes from the PipeWriter
                    var memory = writer.GetMemory(minimumBufferSize);

                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None).ConfigureAwait(false);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    // Tell the PipeWriter how much was read
                    writer.Advance(bytesRead);
                }
                catch
                {
                    break;
                }

                // Make the data available to the PipeReader
                var result = await writer.FlushAsync().ConfigureAwait(false);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Signal to the reader that we're done writing
            writer.Complete();
        }

        private async Task ReadPipeAsync(Socket socket, PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync().ConfigureAwait(false);

                var buffer = result.Buffer;
                SequencePosition? position = null;

                do
                {
                    // Find the EOL                    
                    position = buffer.PositionOf((byte)'\n');

                    if (position != null)
                    {
                        var line = buffer.Slice(0, position.Value);
                        ProcessLine(socket, line);

                        // This is equivalent to position + 1
                        var next = buffer.GetPosition(1, position.Value);

                        // Skip what we've already processed including \n
                        buffer = buffer.Slice(next);
                    }
                }
                while (position != null);

                // We sliced the buffer until no more data could be processed
                // Tell the PipeReader how much we consumed and how much we left to process
                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            reader.Complete();
        }

        private void ProcessLine(Socket socket, in ReadOnlySequence<byte> buffer)
        {
            if (_accept)
            {
                var message = new StringBuilder();
                foreach (var segment in buffer)
                {
                    message.Append(Encoding.UTF8.GetString(segment.Span.ToArray()));
                }
                MessageReceived(new Record<object>
                {
                    EndPoint = socket.RemoteEndPoint,
                    Message = message.ToString().Trim('"'),//due to JsonConvert.SerializeObject
                    Error = string.Empty
                });
            }
        }

        private void ClientDispose()
        {
            _connections.ToList().ForEach(kv =>
            {
                var socketClient = kv.Value;
                if (socketClient?.Connected == true)
                {
                    var endPoint = socketClient.RemoteEndPoint;
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    SocketDisposed(endPoint);
                }
            });
            _connections.Clear();
        }

        void ISocketService.Stop()
        {
            var localEndPoint = _listenerSocket.LocalEndPoint;
            if (_listenerSocket?.Connected == true)
            {
                _listenerSocket.Shutdown(SocketShutdown.Both);
                _listenerSocket.Close();
                _accept = false;

            }
            ClientDispose();
            _disconnected.OnNext(localEndPoint);
        }

        Task ISocketService.SendAsync<T>(T message, Action<Record<T>> errorMessageCallback)
        {
            var buffer = Utility.ObjectToByteArray(message);
            var localEndPoint = _listenerSocket.LocalEndPoint;
            bool ReConnect()
            {
                return Utility.Retry(_retryMax,
                    () => _listenerSocket.ConnectAsync(localEndPoint));
            }

            bool ReSend()
            {
                return Utility.Retry(_retryMax, () => _listenerSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None));
            }

            void callbackInvoke(string errorMessage)
            {
                errorMessageCallback?.Invoke(
                     new Record<T>
                     {
                         EndPoint = localEndPoint,
                         Message = message,
                         Error = $"error:{errorMessage}"
                     });
            }

            if (!Utility.IsConnect(_listenerSocket))
            {
                var result = ReConnect();
                var errorMessage = $"[{localEndPoint}] Disconnected.";
                if (!result)
                {
                    callbackInvoke(errorMessage);
                    _disconnected.OnNext(localEndPoint);
                }
                return Observable.Start(() => Task.FromResult(result))
                    .Do(x => _sender.OnNext
                    (
                     new Record<object>
                     {
                         Message = message,
                         EndPoint = localEndPoint,
                         Error = errorMessage
                     }
                    ),
                    ex =>
                    {
                        callbackInvoke($"{ex.Message}, {ex.StackTrace}");
                        _error.OnNext(new ErrorData("SendAsync", ex));
                    }
                ).ToTask(_cancellation.Token);
            }

            return Observable.Start(() =>
                _listenerSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None)
              )
             .Do(x => _sender.OnNext
             (
                 new Record<object>
                 {
                     Message = message,
                     EndPoint = localEndPoint,
                     Error = ""
                 }
                 ),
                 ex =>
                 {
                     var result = ReSend();
                     if (!result)
                     {
                         var errorMessage = $"{ex.Message}, {ex.StackTrace}";
                         callbackInvoke(errorMessage);
                     }
                 }
              ).ToTask(_cancellation.Token);
        }
    }

    public static class Utility
    {
        internal static byte[] Convert(string message)
        {
            return Encoding.UTF8.GetBytes(message + '\n');//need \n to be endOfLine
            //var header = BitConverter.GetBytes(body.Length);
            //return header.Concat(body).ToArray();
        }

        internal static byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(obj.SerializeToString() + '\n');
        }

        public static bool IsConnect(Socket client)
        {
            var pollResult = client.Poll(250, SelectMode.SelectRead);
            var availableResult = (client.Available == 0);
            if (pollResult && availableResult)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Retry(int maxLoop, Func<Task> action)
        {
            for (int i = 0; i < maxLoop; i++)
            {
                Thread.Sleep(100);
                try
                {
                    action?.Invoke().Wait();
                    return true;
                }
                catch
                {
                    if (i < maxLoop)
                    {
                        continue;
                    }
                    break;
                }
            }
            return false;
        }
        public static async Task<bool> RetryAsync(int maxLoop, Func<Task> action)
        {
            for (int i = 0; i < maxLoop; i++)
            {
                await Task.Delay(100).ConfigureAwait(false);
                try
                {
                    await action?.Invoke();
                    return true;
                }
                catch
                {
                    if (i < maxLoop)
                    {
                        continue;
                    }
                    break;
                }
            }
            return false;
        }
    }

#if NET461 || NETSTANDARD2_0
    internal static class Extensions
    {
        public static Task<int> ReceiveAsync(this Socket socket, Memory<byte> memory, SocketFlags socketFlags)
        {
            var arraySegment = GetArray(memory);
            return SocketTaskExtensions.ReceiveAsync(socket, arraySegment, socketFlags);
        }

        public static string GetString(this Encoding encoding, ReadOnlyMemory<byte> memory)
        {
            var arraySegment = GetArray(memory);
            return encoding.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        private static ArraySegment<byte> GetArray(Memory<byte> memory)
        {
            return GetArray((ReadOnlyMemory<byte>)memory);
        }

        private static ArraySegment<byte> GetArray(ReadOnlyMemory<byte> memory)
        {
            if (!MemoryMarshal.TryGetArray(memory, out var result))
            {
                throw new InvalidOperationException("Buffer backed by array was expected");
            }

            return result;
        }
    }
#endif
}
