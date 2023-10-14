using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Networking.Core;

public interface INetworkConnection
{
    Task ConnectAsync();
    Task SendPacketAsync(IPacket packet);

    event EventHandler<(int PacketID, byte[] Data)> NetworkPacketReceived;
}
