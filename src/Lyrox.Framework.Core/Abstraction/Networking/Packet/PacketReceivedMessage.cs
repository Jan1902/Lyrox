using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet;

public record PacketReceivedMessage<TPacket> : IMessage
    where TPacket : IPacket
{
    public TPacket Packet { get; }

    public PacketReceivedMessage(TPacket packet)
        => Packet = packet;
}
