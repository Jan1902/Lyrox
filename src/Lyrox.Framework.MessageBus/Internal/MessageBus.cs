using System.Collections.Immutable;
using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Base.Messaging.Abstraction.Exceptions;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Base.Messaging.Abstraction.Requests;

namespace Lyrox.Framework.Base.Messaging.Internal;

internal class MessageBus : IMessageBus
{
    private readonly Dictionary<Type, List<object>> _messageHandlers;
    private readonly Dictionary<Type, List<object>> _requestHandlers;

    public MessageBus()
        => (_messageHandlers, _requestHandlers) = (new(), new());

    public Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
    {
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        if (!_messageHandlers.ContainsKey(typeof(TMessage)))
            return Task.CompletedTask;

        var handlers = _messageHandlers[typeof(TMessage)]
            .Select(h => h as IMessageHandler<TMessage>);

        return Task.WhenAll(handlers.Select(h => h!.HandleMessageAsync(message)).ToArray());
    }

    public Task<TResponse[]> GetResponsesAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (!_requestHandlers.ContainsKey(typeof(TRequest)))
            return Task.FromResult(Array.Empty<TResponse>());

        var handlers = _requestHandlers[typeof(TRequest)]
            .Select(h => h as IRequestHandler<TRequest, TResponse>);

        return Task.WhenAll(handlers.Select(h => h!.HandleRequestAsync(request)).ToArray());
    }

    public IDisposable Subscribe<TMessage, THandler>(THandler handler)
        where TMessage : IMessage
        where THandler : IMessageHandler<TMessage>
    {
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));

        if (!_messageHandlers.ContainsKey(typeof(TMessage)))
            _messageHandlers[typeof(TMessage)] = new();

        if (_messageHandlers[typeof(TMessage)].Contains(handler))
            throw new MessagingException("The given handler is already registered for this message type");

        _messageHandlers[typeof(TMessage)].Add(handler);

        return new MessageBusSubscription(
            () => _messageHandlers[typeof(TMessage)].Remove(handler));
    }

    public IDisposable Subscribe<TMessage>(Func<TMessage, Task> callback) where TMessage : IMessage
    {
        if (callback is null)
            throw new ArgumentNullException(nameof(callback));

        return Subscribe<TMessage, IMessageHandler<TMessage>>(new MessageHandleWrapper<TMessage>(callback));
    }

    public IDisposable Subscribe<TRequest, TResponse, THandler>(THandler handler)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
        where THandler : IRequestHandler<TRequest, TResponse>
    {
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));

        if (!_requestHandlers.ContainsKey(typeof(TRequest)))
            _requestHandlers[typeof(TRequest)] = new();

        if (_requestHandlers[typeof(TRequest)].Contains(handler))
            throw new MessagingException("The given handler is already registered for this message type");

        _requestHandlers[typeof(TRequest)].Add(handler);

        return new MessageBusSubscription(
            () => _requestHandlers[typeof(TRequest)].Remove(handler));
    }

    public IDisposable Subscribe<TRequest, TResponse>(Func<TRequest, Task<TResponse>> callback)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        if (callback is null)
            throw new ArgumentNullException(nameof(callback));

        return Subscribe<TRequest, TResponse, IRequestHandler<TRequest, TResponse>>(
            new RequestHandleWrapper<TRequest, TResponse>(callback));
    }

    protected IDisposable SubscribeMessageHandlerInternal(Type messageType, object handler)
    {
        if (messageType is null)
            throw new ArgumentNullException(nameof(messageType));
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));

        if (!messageType.IsAssignableTo(typeof(IMessage)))
            throw new ArgumentException("The given message type is not assignable to IMessage", nameof(messageType));
        if (!handler.GetType().IsAssignableTo(typeof(IMessageHandler<>).MakeGenericType(messageType)))
            throw new ArgumentException("The given handler is not valid for this message type", nameof(handler));

        if (!_messageHandlers.ContainsKey(messageType))
            _messageHandlers[messageType] = new();

        if (_messageHandlers[messageType].Contains(handler))
            throw new MessagingException("The given handler is already registered for this message type");

        _messageHandlers[messageType].Add(handler);

        return new MessageBusSubscription(
            () => _messageHandlers[messageType].Remove(handler));
    }

    protected IDisposable SubscribeRequestHandlerInternal(Type requestType, Type responseType, object handler)
    {
        if (requestType is null)
            throw new ArgumentNullException(nameof(requestType));
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));

        if (!requestType.IsAssignableTo(typeof(IRequest<>).MakeGenericType(responseType)))
            throw new ArgumentException("The given message type is not assignable to IMessage", nameof(requestType));
        if (!handler.GetType().IsAssignableTo(typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType)))
            throw new ArgumentException("The given handler is not valid for this message type", nameof(handler));

        if (!_requestHandlers.ContainsKey(requestType))
            _requestHandlers[requestType] = new();

        if (_requestHandlers[requestType].Contains(handler))
            throw new MessagingException("The given handler is already registered for this message type");

        _requestHandlers[requestType].Add(handler);

        return new MessageBusSubscription(
            () => _requestHandlers[requestType].Remove(handler));
    }

    internal ImmutableDictionary<Type, List<object>> MessageHandlers
        => _messageHandlers.ToImmutableDictionary();

    internal ImmutableDictionary<Type, List<object>> RequestHandlers
        => _requestHandlers.ToImmutableDictionary();
}
