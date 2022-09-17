using Lyrox.Core.Networking.Abstraction.Packet;

namespace Lyrox.Core.Networking.Abstraction.Handler
{
    public interface IPacketHandler<T> where T : IClientBoundNetworkPacket
    {
        void HandlePacket(T networkPacket);
    }
}
