using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Exceptions;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Player.Mojang.PacketHandlers;
using Lyrox.Framework.Player.Mojang.Packets.ClientBound;

namespace Lyrox.Framework.Player
{
    public class PlayerModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<IPhysicsPlayer, PhysicsPlayer>();
            serviceContainer.RegisterType<IPlayerManager, PlayerManager>();
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {

        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == Core.Networking.Types.GameVersion.Mojang)
                networkPacketManager.RegisterNetworkPacketHandler<SynchronizePlayerPosition, PlayerPacketHandler>(0x39);
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }
    }
}
