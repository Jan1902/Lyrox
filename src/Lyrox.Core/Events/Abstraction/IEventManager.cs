using Autofac;

namespace Lyrox.Core.Events.Abstraction
{
    public interface IEventManager
    {
        void PublishEvent<TEvent>(TEvent evt) where TEvent : Event;
        void RegisterEventHandler(Type eventType, object handler);
        void RegisterEventHandler<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : Event;
        void RegisterEventHandler<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;
        void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : Event;
        void RegisterEventHandlersFromContainer(IContainer container);
    }
}
