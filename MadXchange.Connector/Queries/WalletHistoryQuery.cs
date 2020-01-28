using Convey.CQRS.Queries;

namespace MadXchange.Connector.Queries
{
    public class WalletHistoryQuery<T> : IQuery<T>
    {
        public T Data { get; }
    }
}
