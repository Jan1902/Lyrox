using Lyrox.Core.Abstraction;
using Lyrox.Core.Events;
using Lyrox.Core.Events.Implementations;

namespace Lyrox.Networking.EventHandlers
{
    internal class NetworkingEventHandler : IEventHandler<ConnectionEstablishedEvent>
    {
        private readonly INetworkingManager _networkingManager;

        public NetworkingEventHandler(INetworkingManager networkingManager)
        {
            _networkingManager = networkingManager;
        }

        public void HandleEvent(ConnectionEstablishedEvent evt)
            => _networkingManager.SendStartPackets();
    }
}
