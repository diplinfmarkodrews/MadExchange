
using MadXchange.Common.Types;

namespace MadXchange.Common.Mex.Queries
{
    public class WalletHistoryQuery<T> : IQuery<T>
    {
        public T Data { get; }
    }
}
