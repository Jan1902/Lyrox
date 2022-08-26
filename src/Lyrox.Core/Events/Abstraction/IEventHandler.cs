namespace Lyrox.Core.Events.Abstraction
{
    public interface IEventHandler<T> where T : Event
    {
        void HandleEvent(T evt);
    }
}
