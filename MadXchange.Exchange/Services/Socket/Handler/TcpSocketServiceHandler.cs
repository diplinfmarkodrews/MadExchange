using MadXchange.Exchange.Infrastructure.Stores;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MadXchange.Connector.Services;
using MadXchange.Exchange.Contracts;

namespace MadXchange.Exchange.Services.Socket
{
    //public sealed class TcpSocketServiceHandler : WebSocketHandler
    //{
    //    private ISocketService _tcpSocketService;
    //    private IAccountManager _accountManager;

    //    public TcpSocketServiceHandler(ISocketService socketService, IAccountManager accountManager)
    //    {
    //        _tcpSocketService = socketService;
    //        _accountManager = accountManager;
    //    }

    //    public Task OnConnected(Guid socketId)
    //    {

    //        return Task.CompletedTask;
    //    }

    //    public override Task OnDisconnected(WebSocket socket) 
    //    {
    //        return Task.CompletedTask;
    //    }

        

    //    public  async Task CloseAsync(Guid id) 
    //    {
            
    //    }





    //    private async Task SendMessageAsync(Guid socketId, string message, CancellationToken token) 
    //    {
        
    //    }

    //    public override Task OnConnected(WebSocket socket)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public  Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, ReadOnlySpan<char> buffer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task CloseAsync(WebSocket socket)
    //    {
    //        throw new NotImplementedException();
    //    }

        

    //    public override Task SendRequestAsync(Guid socket, SocketRequestDto message, CancellationToken token)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task ReceiveAsync(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}