using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MadXchange.Connector
{
    public interface IClientSocket
    {
        Task ConnectAsync(string IP, int Port);
        void Disconnect();
        bool IsConnected { get; }
        Task SendAsync<T>(T message, int retryMax, Action<Record<T>> errorMessageCallback = null);
    }

    public sealed class ClientSocket : IClientSocket
    {
        private Socket _clientSocket;
                
        public ClientSocket()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            
        }

        private Action<Socket> ShowConnected = (clientSocket) =>
        {
            Console.WriteLine($"[{clientSocket.RemoteEndPoint}] Connected.");
        };

        private Action<EndPoint> ShowDisconnected = (localEndPoint) =>
        {

            Console.WriteLine($"[{localEndPoint}] Disconnected.");
        };

        bool IClientSocket.IsConnected => Utility.IsConnect(_clientSocket);

        async Task IClientSocket.ConnectAsync(string IP, int Port)
        {
            var ipAddress = IPAddress.Parse(IP);
            if (_clientSocket == null)
            {
                _clientSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
            }
            await _clientSocket.ConnectAsync(new IPEndPoint(ipAddress, Port))
                .ContinueWith(tresult =>
                {
                    if (tresult.Status != TaskStatus.Faulted)
                    {
                        ShowConnected(_clientSocket);
                    }
                });
        }

        void IClientSocket.Disconnect()
        {
            var localEndPoint = _clientSocket.LocalEndPoint;
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
            _clientSocket = null;
            ShowDisconnected(localEndPoint);
        }

        Task IClientSocket.SendAsync<T>(T message, int retryMax, Action<Record<T>> errorMessageCallback)
        {
            var buffer = Utility.ObjectToByteArray<T>(message);
            var remoteEndpoint = _clientSocket.RemoteEndPoint;
            bool ReConnect()
            {
                return Utility.Retry(retryMax,
                    () => _clientSocket.ConnectAsync(remoteEndpoint));
            }

            bool ReSend()
            {
                return Utility.Retry(retryMax, () => _clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None));
            }

            void callbackInvoke(string errorMessage)
            {
                errorMessageCallback?.Invoke(
                     new Record<T>
                     {
                         EndPoint = remoteEndpoint,
                         Message = message,
                         Error = $"error:{errorMessage}"
                     });
            }

            if (!Utility.IsConnect(_clientSocket))
            {
                var localEndPoint = _clientSocket.LocalEndPoint;
                var result = ReConnect();
                if (!result)
                {
                    var errorMessage = $"[{localEndPoint}] Disconnected.";
                    callbackInvoke(errorMessage);
                    ShowDisconnected(localEndPoint);
                }
                return Task.FromResult(result);
            }
            return _clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None)
                .ContinueWith(t =>
                {
                    var result = ReSend();
                    if (!result)
                    {
                        var errorMessage = $"{t.Exception.Message}, {t.Exception.StackTrace}";
                        callbackInvoke(errorMessage);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
            //.ContinueWith(t=> t.GetAwaiter().GetResult(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
