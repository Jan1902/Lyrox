using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Shared.Events;

public record ChatMessageReceivedMessage : IMessage
{
    public string Message { get; init; }
    public string Sender { get; init; }

    public ChatMessageReceivedMessage(string message, string sender)
    {
        Message = message;
        Sender = sender;
    }
}
