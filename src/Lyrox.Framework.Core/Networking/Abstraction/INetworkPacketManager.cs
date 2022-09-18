using Autofac;
using Lyrox.Framework.Core.Networking.Abstraction.Packet;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;

namespace Lyrox.Framework.Core.Networking.Abstraction
{
    public interface INetworkPacketManager
    {
        void RegisterNetworkPacketHandler<TPacket, THandler>(int opCode)
            where TPacket : IClientBoundNetworkPacket
            where THandler : IPacketHandler<TPacket>;
        void RegisterNetworkPacketHandler(Type packetType, object handler);
        void RegisterRawNetworkPacketHandler<THandler>(int opCode)
            where THandler : IRawPacketHandler;
        void RegisterRawNetworkPacketHandler(int opCode, object handler);
        void RegisterPacketHandlersFromContainer(IContainer container);
        void HandleNetworkPacket(int opCode, byte[] data);
    }
}
