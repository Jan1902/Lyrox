using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.Events;

internal record ProtocolStateChangedMessage : IMessage
{
    public ProtocolState ProtocolState { get; init; }

    public ProtocolStateChangedMessage(ProtocolState protocolState)
        => ProtocolState = protocolState;
}
