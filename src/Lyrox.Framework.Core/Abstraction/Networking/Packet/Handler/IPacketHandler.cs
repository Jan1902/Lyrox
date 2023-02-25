using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

public interface IPacketHandler<TPacket>
    : IMessageHandler<PacketReceivedMessage<TPacket>>
    where TPacket : IClientBoundNetworkPacket
{
    Task IMessageHandler<PacketReceivedMessage<TPacket>>
        .HandleMessageAsync(PacketReceivedMessage<TPacket> message)
        => HandlePacketAsync(message.Packet);

    Task HandlePacketAsync(TPacket packet);
}
