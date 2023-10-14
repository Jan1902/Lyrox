using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

namespace Lyrox.Framework.Networking.Core;

public static class Extensions
{
    public static void RegisterPacketHandler<TPacket, THandler>(this ServiceContainer serviceContainer, PacketTypeMapping packetMapping, int opCode)
        where TPacket : IPacket
        where THandler : IPacketHandler<TPacket>
    {
        serviceContainer.RegisterType<IMessageHandler<PacketReceivedMessage<TPacket>>, THandler>();
        packetMapping.AddMapping<TPacket>(opCode);
    }

    public static void RegisterRawPacketHandler<THandler>(this ServiceContainer serviceContainer)
        where THandler : IRawPacketHandler
        => serviceContainer.RegisterType<IMessageHandler<RawPacketReceivedMessage>, THandler>();
}
