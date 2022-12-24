using Lyrox.Framework.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Messaging.Internal
{
    internal class MessageHandleWrapper<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        private readonly Func<TMessage, Task> _handleFunction;

        public MessageHandleWrapper(Func<TMessage, Task> handleFunction)
            => _handleFunction = handleFunction;

        public Task HandleMessageAsync(TMessage message)
            => _handleFunction(message);
    }
}
