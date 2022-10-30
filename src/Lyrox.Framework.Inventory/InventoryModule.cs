using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Exceptions;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Core.Networking.Types;
using Lyrox.Framework.Inventory.Mojang;
using Lyrox.Framework.Inventory.Mojang.PacketHandlers;
using Lyrox.Framework.Inventory.Mojang.Packets;

namespace Lyrox.Framework.Inventory
{
    public class InventoryModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                serviceContainer.RegisterType<IInventoryManager, InventoryManager>();
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {
        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                networkPacketManager.RegisterNetworkPacketHandler<SetContainerContent, InventoryPacketHandler>(0x11);
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }
    }
}
