using MadXchange.Exchange.Services.Socket;
using System;
using System.Collections.Concurrent;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface ISocketConnectionStore
    {
       
        bool Contains(Guid id);
        Guid AddSocketConnection(WebSocketConnection socket);
        Guid RemoveSocketConnection(Guid id);        
        WebSocketConnection GetConnection(Guid id);        
    }

    public class SocketConnectionStore : ISocketConnectionStore
    {

        private ConcurrentDictionary<Guid, WebSocketConnection> _socketConnections = new ConcurrentDictionary<Guid, WebSocketConnection>();
        
        /// <summary>
        /// public accessors to the addrress dictionaries
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        public bool Contains(Guid accountId) => _socketConnections.ContainsKey(accountId);
     
        public WebSocketConnection GetConnection(Guid accountId) => _socketConnections[accountId];
        

        public Guid AddSocketConnection(WebSocketConnection socketConnection) 
        {
            try
            {               
                _socketConnections.TryAdd(socketConnection.Id, socketConnection);
                return socketConnection.Id;
            }
            catch 
            {                
                return AddSocketConnectionFailOver(socketConnection, 5);
            }
        }
        //make sure socket gets added => recursive recall
        private Guid AddSocketConnectionFailOver(WebSocketConnection socketConnection, int retry) 
        {
            if (retry > 0)
                return AddSocketConnectionFailOver(socketConnection, retry - 1);
            return Guid.Empty;
        
        }
                         
        /// <summary>
        /// Remove socketconnection and dispose connection and socket object.
        /// Returns either accountId or in case of a public socketconnection an empty Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid RemoveSocketConnection(Guid id) //connection has to be disposed
        {
            WebSocketConnection socket;                                     
            _socketConnections.TryRemove(id, out socket);                        
            var accountId = socket.Id;
            socket.Dispose(true);            
            return accountId;
        }

        
    }
}