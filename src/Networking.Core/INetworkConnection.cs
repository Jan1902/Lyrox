using Lyrox.Core.Networking.Abstraction.Packet;

namespace Lyrox.Networking.Core
{
    public interface INetworkConnection
    {
        Task Connect();
        Task SendPacket(IServerBoundNetworkPacket packet);
    }
}
