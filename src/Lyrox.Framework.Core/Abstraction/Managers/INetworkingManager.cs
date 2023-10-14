using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface INetworkingManager
{
    Task Connect();
    Task SendPacket(IPacket networkPacket);
}
