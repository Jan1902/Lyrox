using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;

namespace Lyrox.Framework.Networking;

public class NetworkingManager : INetworkingManager
{
    private readonly INetworkConnection _networkConnection;
    private readonly INetworkPacketManager _networkPacketManager;

    public NetworkingManager(INetworkConnection networkConnection, INetworkPacketManager networkPacketManager)
    {
        _networkConnection = networkConnection;
        _networkPacketManager = networkPacketManager;
        _networkConnection.NetworkPacketReceived += NetworkPacketReceived;
    }

    private void NetworkPacketReceived(object? sender, (int PacketID, byte[] Data) e)
        => _networkPacketManager.HandleNetworkPacket(e.PacketID, e.Data);

    public async Task Connect()
        => await _networkConnection.Connect();

    public async Task SendPacket(IServerBoundNetworkPacket networkPacket)
        => await _networkConnection.SendPacket(networkPacket);
}
