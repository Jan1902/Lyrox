using Lyrox.Core.Abstraction;
using Lyrox.Networking.Connection;

namespace Lyrox.Networking
{
    public class NetworkingManager : INetworkingManager
    {
        private readonly INetworkConnection _networkConnection;

        public NetworkingManager(INetworkConnection networkConnection)
            => _networkConnection = networkConnection;

        public async Task Connect()
            => await _networkConnection.Connect();

        public async Task SendStartPackets()
        {
            
        }
    }
}
