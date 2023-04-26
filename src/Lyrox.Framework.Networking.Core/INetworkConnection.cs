using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Networking.Core;

public interface INetworkConnection
{
    Task Connect();
    Task SendPacket(IServerBoundNetworkPacket packet);
    event EventHandler<(int PacketID, byte[] Data)> NetworkPacketReceived;
}
