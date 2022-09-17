using Lyrox.Core.Abstraction;
using Lyrox.Core.Networking.Abstraction.Packet;
using Lyrox.Networking.Core;

namespace Lyrox.Networking
{
    public class NetworkingManager : INetworkingManager
    {
        private readonly INetworkConnection _networkConnection;

        public NetworkingManager(INetworkConnection networkConnection) => _networkConnection = networkConnection;

        public async Task Connect()
            => await _networkConnection.Connect();

        public async Task SendPacket(IServerBoundNetworkPacket networkPacket)
            => await _networkConnection.SendPacket(networkPacket);
    }
}
