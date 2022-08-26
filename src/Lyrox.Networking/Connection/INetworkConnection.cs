using Lyrox.Networking.Packets.ServerBound;

namespace Lyrox.Networking.Connection
{
    public interface INetworkConnection
    {
        Task Connect();
        Task SendPacket(ServerBoundPacket packet);
    }
}
