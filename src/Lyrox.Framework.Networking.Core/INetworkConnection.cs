using Lyrox.Framework.Core.Networking.Abstraction.Packet;

namespace Lyrox.Framework.Networking.Core
{
    public interface INetworkConnection
    {
        Task Connect();
        Task SendPacket(IServerBoundNetworkPacket packet);
    }
}
