using Autofac;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Base.Messaging.Internal.Autofac;

internal class AutofacMessageBus : MessageBus
{
    private readonly IComponentContext _componentContext;

    public AutofacMessageBus(IComponentContext componentContext)
        => _componentContext = componentContext;

    internal void Setup()
    {
        RegisterMessageHandlersFromContainer();
        RegisterRequestHandlersFromContainer();
    }

    private void RegisterMessageHandlersFromContainer()
    {
        var messageHandlerInterfaceTypes = _componentContext.ComponentRegistry.Registrations
            .SelectMany(r => r.Activator.LimitType.GetInterfaces()
                .Select(type => (type, GetInheritedGenericInterface(type, typeof(IMessageHandler<>))))
                .Where(i => i.Item2 is not null))
            .Distinct();

        foreach (var interfaceType in messageHandlerInterfaceTypes)
        {
            var handlers = (object[])_componentContext.Resolve(interfaceType.type.MakeArrayType());

            foreach (var handler in handlers)
                SubscribeMessageHandlerInternal(interfaceType.Item2!.GetGenericArguments().First(), handler);
        }
    }

    private void RegisterRequestHandlersFromContainer()
    {
        var requestHandlerInterfaceTypes = _componentContext.ComponentRegistry.Registrations
            .SelectMany(r => r.Activator.LimitType.GetInterfaces()
                .Select(type => (type, GetInheritedGenericInterface(type, typeof(IRequestHandler<,>))))
                .Where(i => i.Item2 is not null))
            .Distinct();

        foreach (var interfaceType in requestHandlerInterfaceTypes)
        {
            var handlers = (object[])_componentContext.Resolve(interfaceType.type.MakeArrayType());

            foreach (var handler in handlers)
                SubscribeRequestHandlerInternal(interfaceType.Item2!.GetGenericArguments().First(),
                    interfaceType!.Item2.GetGenericArguments().Last(),
                    handler);
        }
    }

    private static Type? GetInheritedGenericInterface(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return it;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return givenType;

        var baseType = givenType.BaseType;
        if (baseType == null)
            return default;

        return GetInheritedGenericInterface(baseType, genericType);
    }
}
