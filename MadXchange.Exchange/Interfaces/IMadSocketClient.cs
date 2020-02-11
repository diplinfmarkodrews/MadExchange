using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Connector.Interfaces
{
    public interface IMadSocketClient
    {
        bool IsConnected();

        bool Connect();

        Guid SubScribeSocket<T>(ISocketSubscription<T> subscription);

        void Unsubscribe(Guid subscription);
    }

    public interface ISocketSubscription
    {
        Guid Id { get; }
    }

    public interface ISocketSubscription<T> : ISocketSubscription
    {
        string ClientId { get; }
        public Xchange Exchange { get; }
        public string Subscribtion { get; }
    }
}