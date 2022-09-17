using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Modules.Abstractions;
using Lyrox.Core.Networking.Abstraction;
using Lyrox.Core.Networking.Types;
using Lyrox.WorldData.Mojang.PacketHandlers;
using Lyrox.WorldData.Mojang.Packets;

namespace Lyrox.WorldData
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
            {
                networkPacketManager.RegisterNetworkPacketHandler<ChunkData, WorldDataPacketHandler>(0x1f);
            }
            else
                throw new NotImplementedException("Microsoft Game Version is not supported yet!");
        }
    }
}
