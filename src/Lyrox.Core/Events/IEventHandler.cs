namespace Lyrox.Core.Events
{
    public interface IEventHandler<T> where T : Event
    {
        void HandleEvent(T evt);
    }
}
