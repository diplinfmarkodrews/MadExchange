using MadXchange.Common.Types;
using System.Threading.Tasks;

namespace MadXchange.Common.Handlers
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
