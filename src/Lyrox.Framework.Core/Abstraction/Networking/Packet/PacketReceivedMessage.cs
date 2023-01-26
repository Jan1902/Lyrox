using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

public record PacketReceivedMessage<TPacket> : IMessage
    where TPacket : IClientBoundNetworkPacket
{
    public TPacket Packet { get; }

    public PacketReceivedMessage(TPacket packet)
        => Packet = packet;
}
