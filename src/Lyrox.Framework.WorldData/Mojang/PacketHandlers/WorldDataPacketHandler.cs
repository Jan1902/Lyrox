using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;
using Lyrox.Framework.World.Mojang.Data;
using Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;
using Lyrox.Framework.World.Mojang.Packets;
using Microsoft.Extensions.Logging;

namespace Lyrox.Framework.World.Mojang.PacketHandlers;

internal class WorldDataPacketHandler :
    IPacketHandler<ChunkData>,
    IPacketHandler<BlockUpdate>
{
    private readonly IChunkDataHandler _chunkDataHandler;
    private readonly WorldDataManager _worldDataManager;
    private readonly IGlobalPaletteProvider _globalPaletteProvider;
    private readonly ILogger<WorldDataPacketHandler> _logger;

    public WorldDataPacketHandler(IChunkDataHandler chunkDataHandler, WorldDataManager worldDataManager, IGlobalPaletteProvider globalPaletteProvider, ILogger<WorldDataPacketHandler> logger)
    {
        _chunkDataHandler = chunkDataHandler;
        _worldDataManager = worldDataManager;
        _globalPaletteProvider = globalPaletteProvider;
        _logger = logger;
    }

    public Task HandlePacketAsync(ChunkData chunkData)
    {
        var chunk = _chunkDataHandler.HandleChunkData(chunkData.Data);
        _worldDataManager.SetChunk(chunkData.ChunkX, chunkData.ChunkZ, chunk);

        _logger.LogTrace($"Received Chunk Data for Chunk at {chunkData.ChunkX}/{chunkData.ChunkZ}");
        return Task.CompletedTask;
    }

    public Task HandlePacketAsync(BlockUpdate blockUpdate)
    {
        _worldDataManager.SetBlock(blockUpdate.Location, _globalPaletteProvider.GetStateFromId(blockUpdate.ID));

        _logger.LogTrace($"Received Block Update for Block at {blockUpdate.Location.X}/{blockUpdate.Location.Y}/{blockUpdate.Location.Z}");
        return Task.CompletedTask;
    }
}
