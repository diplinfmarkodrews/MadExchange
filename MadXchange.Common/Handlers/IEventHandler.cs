using MadXchange.Common.Messages;
using System.Threading.Tasks;

namespace MadXchange.Common.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
