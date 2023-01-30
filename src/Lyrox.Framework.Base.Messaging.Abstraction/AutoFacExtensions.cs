using Autofac;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Base.Messaging.Abstraction.Requests;

namespace Lyrox.Framework.Base.Messaging.Abstraction;

public static class AutofacExtensions
{
    public static void RegisterMessageHandler<TMessage, THandler>(this ContainerBuilder builder)
        where TMessage : IMessage
        where THandler : IMessageHandler<TMessage>
        => builder.RegisterType<THandler>()
            .As<IMessageHandler<TMessage>>()
            .InstancePerDependency();

    public static void RegisterRequestHandler<TRequest, TResponse, THandler>(this ContainerBuilder builder)
        where TRequest : IRequest<TResponse>
        where THandler : IRequestHandler<TRequest, TResponse>
        where TResponse : IResponse
        => builder.RegisterType<THandler>()
            .As<IRequestHandler<TRequest, TResponse>>()
            .InstancePerDependency();
}
