using MadXchange.Connector.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface ISocketConnectionStore
    {
        bool Contains(int id);
        bool Contains(Guid id);
        int AddSocketConnection(WebSocketConnection socket);
        Guid RemoveSocket(int id);
        WebSocketConnection GetConnection(WebSocket socket);
    }

    public class SocketConnectionStore : ISocketConnectionStore
    {

        private ConcurrentDictionary<int, WebSocketConnection> _socketConnections = new ConcurrentDictionary<int, WebSocketConnection>();
        private ConcurrentDictionary<Guid, int> _connectionMapper = new ConcurrentDictionary<Guid, int>();
        

        /// <summary>
        /// public accessors to the addrress dictionaries
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id) => _socketConnections.ContainsKey(id);
        public WebSocketConnection GetConnection(int socketId) => _socketConnections.GetValueOrDefault(socketId);
        public WebSocketConnection GetConnection(WebSocket socket) => _socketConnections.GetValueOrDefault(socket.GetHashCode());
        public bool Contains(Guid accountId) => _connectionMapper.GetValueOrDefault(accountId) != default;
        public WebSocket GetSocketById(Guid accountId) => _socketConnections[_connectionMapper[accountId]]?.Socket;
        

        public int AddSocketConnection(WebSocketConnection socketConnection) 
        {
            try
            {
                int socketId = socketConnection.Socket.GetHashCode();
                _socketConnections.TryAdd(socketId, socketConnection);
                _connectionMapper.TryAdd(socketConnection.Id, socketId);
                return socketId;
            }
            catch 
            {                
                return AddSocketConnectionFailOver(socketConnection, 5);
            }
        }
        //make sure socket gets added => recursive recall
        private int AddSocketConnectionFailOver(WebSocketConnection socketConnection, int retry) 
        {
            if (retry > 0)
                return AddSocketConnectionFailOver(socketConnection, retry - 1);
            return 0;
        
        }
                       
        /// <summary>
        /// Remove socketconnection and dispose connection and socket object.
        /// Returns either accountId or in case of a public socketconnection an empty Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid RemoveSocket(int id) //connection has to be disposed
        {
            WebSocketConnection socket;
            _socketConnections.TryRemove(id, out socket);
            if (socket.IsPublic)
            {
                socket.Dispose(true);
                return Guid.Empty;
            }
            var accountId = socket.Id;
            socket.Dispose(true);            
            return accountId;
        }

        
    }
}