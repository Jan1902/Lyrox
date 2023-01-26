using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

public interface IMessageHandler<TMessage> where TMessage : IMessage
{
    Task HandleMessageAsync(TMessage message);
}
