using Lyrox.Core.Abstraction;
using Lyrox.Networking.Connection;
using Lyrox.Networking.Packets.ServerBound;
using Lyrox.Networking.Types;

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
            await _networkConnection.SendPacket((int)OPHandshaking.Handshake, new Handshake(760, "localhost", 25565, 2).Build());
            await _networkConnection.SendPacket((int)OPLogin.LoginStart, new LoginStart("Botfried").Build());
        }
    }
}
