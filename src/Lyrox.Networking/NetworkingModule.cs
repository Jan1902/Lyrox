using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Events.Implementations;
using Lyrox.Core.Modules.Abstractions;
using Lyrox.Core.Networking.Abstraction;
using Lyrox.Core.Networking.Types;
using Lyrox.Networking.Core;
using Lyrox.Networking.Mojang;
using Lyrox.Networking.Mojang.EventHandlers;
using Lyrox.Networking.Mojang.PacketHandlers;
using Lyrox.Networking.Mojang.Packets.ClientBound;

namespace Lyrox.Networking
{
    public class NetworkingModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<INetworkingManager, NetworkingManager>();
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                serviceContainer.RegisterType<INetworkConnection, NetworkConnection>();
            }
            else
                throw new NotImplementedException("Microsoft Game Version is not supported yet!");
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                eventManager.RegisterEventHandler<ConnectionEstablishedEvent, MojangNetworkingEventHandler>();
            }
            else
                throw new NotImplementedException("Microsoft Game Version is not supported yet!");
        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                networkPacketManager.RegisterRawNetworkPacketHandler<MojangNetworkingPacketHandler>(0x00);
                networkPacketManager.RegisterRawNetworkPacketHandler<MojangNetworkingPacketHandler>(0x02);
                networkPacketManager.RegisterNetworkPacketHandler<KeepAliveCB, MojangNetworkingPacketHandler>(0x1E);
            }
            else
                throw new NotImplementedException("Microsoft Game Version is not supported yet!");
        }
    }
}
