using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Helpers;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
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
        WebSocketConnection GetConnection(Guid id);
        WebSocketConnection GetConnection(int socketId);
    }

    public class SocketConnectionStore : ISocketConnectionStore
    {

        private ConcurrentDictionary<Guid, WebSocketConnection> _socketConnections = new ConcurrentDictionary<Guid, WebSocketConnection>();
        private ConcurrentDictionary<int, Guid> _connectionMapper = new ConcurrentDictionary<int, Guid>();
        private Dictionary<int, TaskCompletionSource<SocketInvocationResult>> _waitingRemoteInvocations = new Dictionary<int, TaskCompletionSource<SocketInvocationResult>>();

        /// <summary>
        /// public accessors to the addrress dictionaries
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id) => _connectionMapper.ContainsKey(id);
        public bool Contains(Guid accountId) => _socketConnections.ContainsKey(accountId);
        public WebSocketConnection GetConnection(int socketId) => _socketConnections.GetValueOrDefault(_connectionMapper.GetValueOrDefault(socketId));
        public WebSocketConnection GetConnection(WebSocket socket) => _socketConnections.GetValueOrDefault(_connectionMapper.GetValueOrDefault(socket.GetHashCode()));       
        public WebSocketConnection GetConnection(Guid accountId) => _socketConnections[accountId];
        

        public int AddSocketConnection(WebSocketConnection socketConnection) 
        {
            try
            {

                int socketId = socketConnection.Socket.GetID();
                _socketConnections.TryAdd(socketConnection.Id, socketConnection);
                _connectionMapper.TryAdd(socketId, socketConnection.Id);
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
            Guid gId;
            
            _connectionMapper.TryRemove(id, out gId);
            _socketConnections.TryRemove(gId, out socket);
            
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