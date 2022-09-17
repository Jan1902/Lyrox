using Autofac;
using Lyrox.Core.Networking.Abstraction.Handler;
using Lyrox.Core.Networking.Abstraction.Packet;

namespace Lyrox.Core.Networking.Abstraction
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
