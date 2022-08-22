using Autofac;
using Lyrox.Core.Abstraction;
using Lyrox.Core.Events;
using Lyrox.Core.Events.Implementations;
using Lyrox.Core.Modules;
using Lyrox.Networking.Connection;
using Lyrox.Networking.EventHandlers;
using Lyrox.Networking.Parsing;

namespace Lyrox.Networking
{
    public class NetworkingModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NetworkingManager>().As<INetworkingManager>();
            builder.RegisterType<NetworkConnection>().As<INetworkConnection>();
            builder.RegisterType<NetworkPacketParser>().As<INetworkPacketParser>();
        }

        public void RegisterEventHandlers(IEventManager eventManager)
        {
            eventManager.RegisterEventHandler<ConnectionEstablishedEvent, NetworkingEventHandler>();
        }
    }
}
