using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Lyrox.Framework.WorldData.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Packets;

namespace Lyrox.Framework.WorldData.Mojang.PacketHandlers
{
    internal class WorldDataPacketHandler : IPacketHandler<ChunkData>
    {
        private readonly IChunkDataHandler _chunkDataHandler;
        private readonly IWorldDataManager _worldDataManager;

        public WorldDataPacketHandler(IChunkDataHandler chunkDataHandler, IWorldDataManager worldDataManager)
        {
            _chunkDataHandler = chunkDataHandler;
            _worldDataManager = worldDataManager;
        }

        public Task HandlePacket(ChunkData networkPacket)
        {
            var chunk = _chunkDataHandler.HandleChunkData(networkPacket.Data);
            _worldDataManager.SetChunk(networkPacket.ChunkX, networkPacket.ChunkZ, chunk);
            return Task.CompletedTask;
        }
    }
}
