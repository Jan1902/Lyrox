namespace Lyrox.Framework.Core.Events.Abstraction
{
    public interface IEventHandler<T> where T : EventBase
    {
        Task HandleEvent(T evt);
    }
}
