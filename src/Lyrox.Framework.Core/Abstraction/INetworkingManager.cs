using Lyrox.Framework.Core.Networking.Abstraction.Packet;

namespace Lyrox.Framework.Core.Abstraction
{
    public interface INetworkingManager
    {
        Task Connect();
        Task SendPacket(IServerBoundNetworkPacket networkPacket);
    }
}
