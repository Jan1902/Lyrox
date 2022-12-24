using Lyrox.Framework.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Messaging.Abstraction.Handlers
{
    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        Task HandleMessageAsync(TMessage message);
    }
}
