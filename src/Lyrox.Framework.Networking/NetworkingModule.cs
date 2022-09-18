using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Core.Exceptions;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Core.Networking.Types;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.EventHandlers;
using Lyrox.Framework.Networking.Mojang.PacketHandlers;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;

namespace Lyrox.Framework.Networking
{
    public class NetworkingModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<INetworkingManager, NetworkingManager>();
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                serviceContainer.RegisterType<INetworkConnection, NetworkConnection>();
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                eventManager.RegisterEventHandler<ConnectionEstablishedEvent, MojangNetworkingEventHandler>();
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
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
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }
    }
}
