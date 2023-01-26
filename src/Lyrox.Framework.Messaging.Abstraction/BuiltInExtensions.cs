using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Base.Messaging.Abstraction.Requests;
using Lyrox.Framework.Base.Shared;

namespace Lyrox.Framework.Base.Messaging.Abstraction;

public static class BuiltInExtensions
{
    public static void RegisterMessageHandler<TMessage, THandler>(this ServiceContainer container)
        where TMessage : IMessage
        where THandler : IMessageHandler<TMessage>
        => container.RegisterType<IMessageHandler<TMessage>, THandler>();

    public static void RegisterRequestHandler<TRequest, TResponse, THandler>(this ServiceContainer container)
        where TRequest : IRequest<TResponse>
        where THandler : IRequestHandler<TRequest, TResponse>
        where TResponse : IResponse
        => container.RegisterType<IRequestHandler<TRequest, TResponse>, THandler>();
}
