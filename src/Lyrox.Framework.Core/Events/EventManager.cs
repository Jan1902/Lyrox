using System.Collections.Concurrent;
using Autofac;
using Lyrox.Framework.Core.Events.Abstraction;

namespace Lyrox.Framework.Core.Events
{
    public class EventManager : IEventManager
    {
        private readonly ConcurrentDictionary<Type, List<object>> _handlers;
        private readonly ContainerBuilder _containerBuilder;
        private readonly ConcurrentDictionary<Type, List<object>> _methodHandlers;

        private readonly ConcurrentDictionary<Type, List<Type>> _handlerMapping;

        public EventManager(ContainerBuilder containerBuilder)
        {
            _handlers = new();
            _handlerMapping = new();
            _methodHandlers = new();

            _containerBuilder = containerBuilder;
        }

        public EventSubscription RegisterEventHandler<TEvent, THandler>()
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>
        {
            if (!_handlerMapping.Values.Any(l => l.Contains(typeof(THandler))))
                _containerBuilder.RegisterType<THandler>().InstancePerLifetimeScope();

            if (!_handlerMapping.ContainsKey(typeof(TEvent)))
                _handlerMapping[typeof(TEvent)] = new();

            _handlerMapping[typeof(TEvent)].Add(typeof(THandler));

            var subscription = new EventSubscription();
            subscription.Disposed += (s, e) =>
            {
                _handlerMapping[typeof(TEvent)].Remove(typeof(THandler));
                _handlers[typeof(TEvent)].RemoveAll(h => h is THandler);
            };
            return subscription;
        }

        public void RegisterEventHandlersFromContainer(IContainer container)
        {
            foreach (var eventType in _handlerMapping.Keys)
            {
                foreach (var handlerType in _handlerMapping[eventType])
                {
                    var handler = container.Resolve(handlerType);
                    RegisterEventHandler(eventType, handler);
                }
            }
        }

        public EventSubscription RegisterEventHandler(Type eventType, object handler)
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

            var subscription = new EventSubscription();
            subscription.Disposed += (s, e) => _handlers[eventType].Remove(handler);
            return subscription;
        }

        public EventSubscription RegisterEventHandler<T>(IEventHandler<T> eventHandler)
            where T : EventBase
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            return RegisterEventHandler(typeof(T), eventHandler);
        }

        public async Task PublishEvent<T>(T evt)
            where T : EventBase
        {
            var tasks = new List<Task>();
            if (_handlers.ContainsKey(typeof(T)))
                tasks.AddRange(_handlers[typeof(T)].Select(h => ((IEventHandler<T>)h).HandleEvent(evt)));

            if (_methodHandlers.ContainsKey(typeof(T)))
                tasks.AddRange(_methodHandlers[typeof(T)].Select(h => ((Func<T, Task>)h).Invoke(evt)));

            await Task.WhenAll(tasks);
        }

        public EventSubscription RegisterEventHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : EventBase
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlers = new List<object>();

            if (_methodHandlers.ContainsKey(typeof(TEvent)))
                handlers = _methodHandlers[typeof(TEvent)];
            else
                _methodHandlers[typeof(TEvent)] = handlers;

            handlers.Add(handler);

            var subscription = new EventSubscription();
            subscription.Disposed += (s, e) => _methodHandlers[typeof(TEvent)].Remove(handler);
            return subscription;
        }

        public EventSubscription RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : EventBase
            => RegisterEventHandler<TEvent>((evt) => Task.Factory.StartNew(() => handler.Invoke(evt)));
    }
}
