using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Networking.Connection;

namespace Lyrox.Networking
{
    public class NetworkingManager : INetworkingManager
    {
        private readonly INetworkConnection _networkConnection;
        private readonly LyroxConfiguration _lyroxConfiguration;

        public NetworkingManager(INetworkConnection networkConnection, LyroxConfiguration lyroxConfiguration)
        {
            _networkConnection = networkConnection;
            _lyroxConfiguration = lyroxConfiguration;
        }

        public async Task Connect()
            => await _networkConnection.Connect();
    }
}
