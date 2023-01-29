using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Shared.Exceptions;
using Lyrox.Framework.Shared.Types;
using Lyrox.Framework.World.Mojang.Data;
using Lyrox.Framework.World.Mojang.Data.Palette;
using Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;
using Lyrox.Framework.World.Mojang.PacketHandlers;
using Lyrox.Framework.World.Mojang.Packets;

namespace Lyrox.Framework.World;

public class WorldDataModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
    {
        serviceContainer.RegisterType<IWorldDataManager, WorldDataManager>();
        if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
        {
            serviceContainer.RegisterType<IChunkDataHandler, ChunkDataHandler>();
            serviceContainer.RegisterType<IGlobalPaletteProvider, FileGlobalPaletteProvider>();
            serviceContainer.RegisterType<IPaletteFactory, PaletteFactory>();

            serviceContainer.RegisterPacketHandler<ChunkData, WorldDataPacketHandler>(packetMapping, 0x21);
        }
        else
            throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
    }
}
