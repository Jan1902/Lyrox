using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Events.Implementations;
using Lyrox.Core.Modules.Abstractions;
using Lyrox.Networking.Connection;
using Lyrox.Networking.EventHandlers;
using Lyrox.Networking.Events;
using Lyrox.Networking.Packets;

namespace Lyrox.Networking
{
    public class NetworkingModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<INetworkingManager, NetworkingManager>();
            serviceContainer.RegisterType<INetworkConnection, NetworkConnection>();
            serviceContainer.RegisterType<IPacketHandler, PacketHandler>();
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {
            eventManager.RegisterEventHandler<ConnectionEstablishedEvent, NetworkingEventHandler>();
            eventManager.RegisterEventHandler<LoginSucessfulEvent, NetworkingEventHandler>();
            eventManager.RegisterEventHandler<KeepAliveEvent, NetworkingEventHandler>();
        }
    }
}
