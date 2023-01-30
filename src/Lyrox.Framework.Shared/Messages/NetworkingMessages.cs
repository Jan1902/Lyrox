using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Shared.Messages;

public record ConnectionEstablishedMessage : IMessage;
public record ConnectionTerminatedMessage : IMessage;
