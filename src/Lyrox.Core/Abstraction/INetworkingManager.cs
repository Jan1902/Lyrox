using Lyrox.Core.Networking.Abstraction.Packet;

namespace Lyrox.Core.Abstraction
{
    public interface INetworkingManager
    {
        Task Connect();
        Task SendPacket(IServerBoundNetworkPacket networkPacket);
    }
}
