using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Inventory.Mojang;
using Lyrox.Framework.Inventory.Mojang.PacketHandlers;
using Lyrox.Framework.Inventory.Mojang.Packets;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Shared.Exceptions;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Inventory;

public class InventoryModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
    {
        if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
        {
            serviceContainer.RegisterType<IInventoryManager, InventoryManager>();
            serviceContainer.RegisterPacketHandler<SetContainerContent, InventoryPacketHandler>(packetMapping, 0x11);
        }
        else
            throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
    }
}
