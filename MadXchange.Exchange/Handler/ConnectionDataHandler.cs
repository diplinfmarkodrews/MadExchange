using MadXchange.Connector.Domain.Models;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Handler
{

    public interface IConnectionDataHandler
    {
        Task HandleDataAsync(SocketMsgPack socketMsgPack);
    }
    /// <summary>
    /// Handles incoming requests of websocketconnections.
    /// Just an abstract  base class
    /// </summary>
    public abstract class ConnectionDataHandler : IConnectionDataHandler
    {
             
        public abstract Task HandleDataAsync(SocketMsgPack socketMsgPack);
       
    }
}

