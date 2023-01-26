using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;
using Lyrox.Framework.World.Mojang.Data;
using Lyrox.Framework.World.Mojang.Packets;

namespace Lyrox.Framework.World.Mojang.PacketHandlers;

internal class WorldDataPacketHandler : IPacketHandler<ChunkData>
{
    private readonly IChunkDataHandler _chunkDataHandler;
    private readonly IWorldDataManager _worldDataManager;

    public WorldDataPacketHandler(IChunkDataHandler chunkDataHandler, IWorldDataManager worldDataManager)
    {
        _chunkDataHandler = chunkDataHandler;
        _worldDataManager = worldDataManager;
    }

    public Task HandlePacketAsync(ChunkData networkPacket)
    {
        var chunk = _chunkDataHandler.HandleChunkData(networkPacket.Data);
        _worldDataManager.SetChunk(networkPacket.ChunkX, networkPacket.ChunkZ, chunk);
        return Task.CompletedTask;
    }
}
