using Autofac;

namespace Lyrox.Core.Events
{
    public interface IEventManager
    {
        void PublishEvent<T>(T evt) where T : Event;
        void RegisterEventHandler(Type eventType, object handler);
        void RegisterEventHandler<T>(IEventHandler<T> eventHandler) where T : Event;
        void RegisterEventHandler<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;
        void RegisterEventHandlersFromContainer(IContainer container);
    }
}