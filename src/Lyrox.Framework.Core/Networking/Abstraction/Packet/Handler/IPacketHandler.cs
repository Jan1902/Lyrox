using Lyrox.Framework.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler
{
    public interface IPacketHandler<T> where T : IClientBoundNetworkPacket
    {
        Task HandlePacket(T networkPacket);
    }

    public interface IPacketMessageHandler<TPacket> : IMessageHandler<PacketReceivedMessage<TPacket>>
        where TPacket : IClientBoundNetworkPacket
    {
        Task IMessageHandler<PacketReceivedMessage<TPacket>>.HandleMessageAsync(PacketReceivedMessage<TPacket> message)
            => HandlePacketAsync(message.Packet);

        Task HandlePacketAsync(TPacket packet);
    }

    public record PacketReceivedMessage<TPacket> : IMessage
        where TPacket : IClientBoundNetworkPacket
    {
        public TPacket Packet { get; }

        public PacketReceivedMessage(TPacket packet)
            => Packet = packet;
    }
}
