using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Base.Messaging.Internal;

internal class MessageHandleWrapper<TMessage> : IMessageHandler<TMessage>
    where TMessage : IMessage
{
    private readonly Func<TMessage, Task> _handleFunction;

    public MessageHandleWrapper(Func<TMessage, Task> handleFunction)
        => _handleFunction = handleFunction;

    public Task HandleMessageAsync(TMessage message)
        => _handleFunction(message);
}
