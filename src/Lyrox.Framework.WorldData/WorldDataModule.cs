using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Core.Networking.Types;
using Lyrox.Framework.WorldData.Mojang.PacketHandlers;
using Lyrox.Framework.WorldData.Mojang.Packets;

namespace Lyrox.Framework.WorldData
{
    public class WorldDataModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<IWorldDataManager, WorldDataManager>();
        }

        public void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {

        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                networkPacketManager.RegisterNetworkPacketHandler<ChunkData, WorldDataPacketHandler>(0x1f);
            else
                throw new NotImplementedException("Microsoft Game Version is not supported yet!");
        }
    }
}
