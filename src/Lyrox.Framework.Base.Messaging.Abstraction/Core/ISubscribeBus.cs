using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Base.Messaging.Abstraction.Requests;

namespace Lyrox.Framework.Base.Messaging.Abstraction.Core;

public interface ISubscribeBus
{
    IDisposable Subscribe<TMessage, THandler>(THandler handler)
        where TMessage : IMessage
        where THandler : IMessageHandler<TMessage>;

    IDisposable Subscribe<TMessage>(Func<TMessage, Task> callback)
        where TMessage : IMessage;

    IDisposable Subscribe<TRequest, TResponse, THandler>(THandler handler)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
        where THandler : IRequestHandler<TRequest, TResponse>;

    IDisposable Subscribe<TRequest, TResponse>(Func<TRequest, Task<TResponse>> callback)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;
}
