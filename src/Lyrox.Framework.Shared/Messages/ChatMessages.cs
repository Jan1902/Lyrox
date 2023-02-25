using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Shared.Messages;

public record ChatMessageReceivedMessage(string Message, string Sender) : IMessage;
