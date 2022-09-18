using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang.Events;
using Lyrox.Framework.Networking.Mojang.Packets.ServerBound;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.EventHandlers
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
