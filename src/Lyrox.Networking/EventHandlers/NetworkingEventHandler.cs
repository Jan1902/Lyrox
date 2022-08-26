using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Events.Implementations;
using Lyrox.Networking.Connection;
using Lyrox.Networking.Events;
using Lyrox.Networking.Packets;
using Lyrox.Networking.Packets.ServerBound;
using Lyrox.Networking.Types;

namespace Lyrox.Networking.EventHandlers
{
    internal class NetworkingEventHandler
        : IEventHandler<ConnectionEstablishedEvent>,
        IEventHandler<LoginSucessfulEvent>,
        IEventHandler<KeepAliveEvent>
    {
        private readonly INetworkConnection _networkConnection;
        private readonly IPacketHandler _packetHandler;
        private readonly LyroxConfiguration _configuration;

        public NetworkingEventHandler(INetworkConnection networkConnection, IPacketHandler packetHandler, LyroxConfiguration configuration)
        {
            _networkConnection = networkConnection;
            _packetHandler = packetHandler;
            _configuration = configuration;
        }

        public void HandleEvent(ConnectionEstablishedEvent evt)
        {
            _networkConnection.SendPacket(new Handshake(759, _configuration.IPAdress, (ushort)_configuration.Port, 2));
            _networkConnection.SendPacket(new LoginStart(_configuration.Username));
            _packetHandler.SetProtocolState(ProtocolState.Login);
        }

        public void HandleEvent(LoginSucessfulEvent evt)
            => _packetHandler.SetProtocolState(ProtocolState.Play);

        public void HandleEvent(KeepAliveEvent evt)
            => _networkConnection.SendPacket(new KeepAlive(evt.KeepAliveID));
    }
}
