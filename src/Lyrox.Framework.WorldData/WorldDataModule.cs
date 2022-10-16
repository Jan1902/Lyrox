using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Exceptions;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Core.Networking.Types;
using Lyrox.Framework.WorldData.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Data.Palette;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;
using Lyrox.Framework.WorldData.Mojang.PacketHandlers;
using Lyrox.Framework.WorldData.Mojang.Packets;

namespace Lyrox.Framework.WorldData
{
    public class WorldDataModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<IWorldDataManager, WorldDataManager>();
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                serviceContainer.RegisterType<IChunkDataHandler, ChunkDataHandler>();
                serviceContainer.RegisterType<IGlobalPaletteProvider, FileGlobalPaletteProvider>();
                serviceContainer.RegisterType<IPaletteFactory, PaletteFactory>();
            }
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                networkPacketManager.RegisterNetworkPacketHandler<ChunkData, WorldDataPacketHandler>(0x21);
            }
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }
    }
}
