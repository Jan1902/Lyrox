using Autofac;
using Lyrox.Framework.Core.Events.Abstraction;

namespace Lyrox.Framework.Core.Events
{
    public class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<object>> _handlers;
        private readonly ContainerBuilder _containerBuilder;
        private readonly Dictionary<Type, List<object>> _methodHandlers;

        private readonly Dictionary<Type, List<Type>> _handlerMapping;

        public EventManager(ContainerBuilder containerBuilder)
        {
            _handlers = new();
            _handlerMapping = new();
            _methodHandlers = new();

            _containerBuilder = containerBuilder;
        }

        public void RegisterEventHandler<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            if (!_handlerMapping.Values.Any(l => l.Contains(typeof(THandler))))
                _containerBuilder.RegisterType<THandler>().InstancePerLifetimeScope();

            if (!_handlerMapping.ContainsKey(typeof(TEvent)))
                _handlerMapping[typeof(TEvent)] = new();

            _handlerMapping[typeof(TEvent)].Add(typeof(THandler));
        }

        public void RegisterEventHandlersFromContainer(IContainer container)
        {
            foreach (var eventType in _handlerMapping.Keys)
                foreach (var handlerType in _handlerMapping[eventType])
                {
                    var handler = container.Resolve(handlerType);
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
                foreach (var handler in _handlers[typeof(T)]
                    .Select(h => h as IEventHandler<T>)) handler?.HandleEvent(evt);

            if (_methodHandlers.ContainsKey(typeof(T)))
                foreach (var handler in _methodHandlers[typeof(T)]
                    .Select(h => h as Action<T>)) handler?.Invoke(evt);
        }

        public void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : Event
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlers = new List<object>();

            if (_methodHandlers.ContainsKey(typeof(TEvent)))
                handlers = _methodHandlers[typeof(TEvent)];
            else
                _methodHandlers[typeof(TEvent)] = handlers;

            handlers.Add(handler);
        }
    }
}
