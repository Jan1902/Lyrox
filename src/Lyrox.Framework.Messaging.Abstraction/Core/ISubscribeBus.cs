using Lyrox.Framework.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Messaging.Abstraction.Core
{
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
}
