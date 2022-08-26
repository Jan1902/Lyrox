using Autofac;
using Lyrox.Core.Events.Abstraction;

namespace Lyrox.Core.Events
{
    public class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<object>> _handlers;
        private readonly ContainerBuilder _containerBuilder;

        private readonly Dictionary<Type, Type> _handlerMapping;

        public EventManager(ContainerBuilder containerBuilder)
        {
            _handlers = new();
            _handlerMapping = new();

            _containerBuilder = containerBuilder;
        }

        public void RegisterEventHandler<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            if (!_handlerMapping.ContainsValue(typeof(THandler)))
                _containerBuilder.RegisterType<THandler>().InstancePerLifetimeScope();
            _handlerMapping[typeof(TEvent)] = typeof(THandler);
        }

        public void RegisterEventHandlersFromContainer(IContainer container)
        {
            foreach (var eventType in _handlerMapping.Keys)
            {
                var handler = container.Resolve(_handlerMapping[eventType]);
                RegisterEventHandler(eventType, handler);
            }
        }

        public void RegisterEventHandler(Type eventType, object handler)
        {
            if (eventType == null)
                throw new ArgumentNullException(nameof(eventType));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlers = new List<object>();

            if (_handlers.ContainsKey(eventType))
                handlers = _handlers[eventType];
            else
                _handlers[eventType] = handlers;

            handlers.Add(handler);
        }

        public void RegisterEventHandler<T>(IEventHandler<T> eventHandler)
            where T : Event
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            RegisterEventHandler(typeof(T), eventHandler);
        }

        public void PublishEvent<T>(T evt)
            where T : Event
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                foreach (var handler in _handlers[typeof(T)]
                    .Select(h => h as IEventHandler<T>)) handler?.HandleEvent(evt);
            }
        }
    }
}
