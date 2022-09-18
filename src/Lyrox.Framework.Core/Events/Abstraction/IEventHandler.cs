using Lyrox.Framework.Core.Events;

namespace Lyrox.Framework.Core.Events.Abstraction
{
    public interface IEventHandler<T> where T : Event
    {
        void HandleEvent(T evt);
    }
}
