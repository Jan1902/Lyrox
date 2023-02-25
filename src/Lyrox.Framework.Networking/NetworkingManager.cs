using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;

namespace Lyrox.Framework.Networking;

public class NetworkingManager : INetworkingManager
{
    private readonly INetworkConnection _networkConnection;

    public NetworkingManager(INetworkConnection networkConnection) => _networkConnection = networkConnection;

    public async Task Connect()
        => await _networkConnection.Connect();

    public async Task SendPacket(IServerBoundNetworkPacket networkPacket)
        => await _networkConnection.SendPacket(networkPacket);
}
