using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Events.Implementations;
using Lyrox.Networking.Core;
using Lyrox.Networking.Mojang.Events;
using Lyrox.Networking.Mojang.Packets.ServerBound;
using Lyrox.Networking.Mojang.Types;

namespace Lyrox.Networking.Mojang.EventHandlers
{
    public class MojangNetworkingEventHandler
        : IEventHandler<ConnectionEstablishedEvent>
    {
        private readonly INetworkConnection _networkConnection;
        private readonly IEventManager _eventManager;
        private readonly LyroxConfiguration _configuration;

        public MojangNetworkingEventHandler(INetworkConnection networkConnection, LyroxConfiguration configuration, IEventManager eventManager)
        {
            _networkConnection = networkConnection;
            _configuration = configuration;
            _eventManager = eventManager;
        }

        public void HandleEvent(ConnectionEstablishedEvent evt)
        {
            _networkConnection.SendPacket(new Handshake(759, _configuration.IPAdress, (ushort)_configuration.Port, ProtocolState.Login));
            _networkConnection.SendPacket(new LoginStart(_configuration.Username));
            _eventManager.PublishEvent(new ProtocolStateChanged(ProtocolState.Login));
        }
    }
}
