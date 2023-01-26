﻿using Autofac;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Base.Messaging.Internal.Autofac;

internal class AutofacMessageBus : MessageBus
{
    private readonly IComponentContext _componentContext;

    public AutofacMessageBus(IComponentContext componentContext)
    {
        _componentContext = componentContext;

        RegisterMessageHandlersFromContainer();
        RegisterRequestHandlersFromContainer();
    }

    private void RegisterMessageHandlersFromContainer()
    {
        var messageHandlerInterfaceTypes = _componentContext.ComponentRegistry.Registrations
            .SelectMany(r => r.Activator.LimitType.GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IMessageHandler<>))))
            .Where(i => i != null)
            .Distinct();

        foreach (var interfaceType in messageHandlerInterfaceTypes)
        {
            var handlers = (object[])_componentContext.Resolve(interfaceType.MakeArrayType());

            foreach (var handler in handlers)
                SubscribeMessageHandlerInternal(interfaceType!.GetGenericArguments().First(), handler);
        }
    }

    private void RegisterRequestHandlersFromContainer()
    {
        var requestHandlerInterfaceTypes = _componentContext.ComponentRegistry.Registrations
            .SelectMany(r => r.Activator.LimitType.GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>))))
            .Where(i => i != null)
            .Distinct();

        foreach (var interfaceType in requestHandlerInterfaceTypes)
        {
            var handlers = (object[])_componentContext.Resolve(interfaceType.MakeArrayType());

            foreach (var handler in handlers)
                SubscribeRequestHandlerInternal(interfaceType!.GetGenericArguments().First(),
                    interfaceType!.GetGenericArguments().Last(),
                    handler);
        }
    }
}
