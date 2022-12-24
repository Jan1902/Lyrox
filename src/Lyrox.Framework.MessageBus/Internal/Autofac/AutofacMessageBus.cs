using Autofac;
using Lyrox.Framework.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Messaging.Internal.Autofac
{
    internal class AutoFacMessageBus : MessageBus
    {
        private readonly IComponentContext _componentContext;

        public AutoFacMessageBus(IComponentContext componentContext)
        {
            _componentContext = componentContext;
            RegisterHandlersFromContainer();
        }

        private void RegisterHandlersFromContainer()
        {
            var messageHandlerInterfaceTypes = _componentContext.ComponentRegistry.Registrations
                .SelectMany(r => r.Activator.LimitType.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IMessageHandler<>))))
                .Where(i => i != null)
                .Distinct();

            foreach (var interfaceType in messageHandlerInterfaceTypes)
            {
                var handlers = (object[])_componentContext.Resolve(interfaceType.MakeArrayType());
                var tst = interfaceType!.GetGenericArguments().First();
                foreach (var handler in handlers)
                    SubscribeMessageHandlerInternal(interfaceType!.GetGenericArguments().First(), handler);
            }
        }
    }
}
