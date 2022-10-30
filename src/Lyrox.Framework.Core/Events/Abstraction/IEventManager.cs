using Autofac;

namespace Lyrox.Framework.Core.Events.Abstraction
{
    public interface IEventManager
    {
        Task PublishEvent<TEvent>(TEvent evt) where TEvent : EventBase;
        EventSubscription RegisterEventHandler(Type eventType, object handler);
        EventSubscription RegisterEventHandler<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : EventBase;
        EventSubscription RegisterEventHandler<TEvent, THandler>()
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>;
        EventSubscription RegisterEventHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : EventBase;
        EventSubscription RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : EventBase;
        void RegisterEventHandlersFromContainer(IContainer container);
    }
}
