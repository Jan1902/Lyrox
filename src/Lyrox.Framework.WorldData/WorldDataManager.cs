using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.World;

public class WorldDataManager : IWorldDataManager
{
    private readonly Dictionary<(int X, int Z), Chunk> _chunks;

    public WorldDataManager()
        => _chunks = new();

    public BlockState? GetBlock(int blockX, int blockY, int blockZ)
    {
        var chunkX = (int)Math.Floor(blockX / 16f);
        var chunkY = (int)Math.Floor(blockY / 16f) + 4;
        var chunkZ = (int)Math.Floor(blockZ / 16f);

        var xOffset = blockX % 16;
        var yOffset = blockY % 16;
        var zOffset = blockZ % 16;

        xOffset = xOffset >= 0 ? xOffset : 16 + xOffset;
        yOffset = yOffset >= 0 ? yOffset : 16 + yOffset;
        zOffset = zOffset >= 0 ? zOffset : 16 + zOffset;

        return GetChunk(chunkX, chunkZ)?
            .Sections[chunkY]
            .BlockStates[xOffset, yOffset, zOffset];
    }

    public void SetBlock(int blockX, int blockY, int blockZ, BlockState blockState)
    {
        var chunkX = (int)Math.Floor(blockX / 16f);
        var chunkY = (int)Math.Floor(blockY / 16f) + 4;
        var chunkZ = (int)Math.Floor(blockZ / 16f);

        var xOffset = blockX % 16;
        var yOffset = blockY % 16;
        var zOffset = blockZ % 16;

        xOffset = xOffset >= 0 ? xOffset : 16 + xOffset;
        yOffset = yOffset >= 0 ? yOffset : 16 + yOffset;
        zOffset = zOffset >= 0 ? zOffset : 16 + zOffset;

        _chunks[(chunkX, chunkZ)]
            .Sections[chunkY]
            .BlockStates[xOffset, yOffset, zOffset] = blockState;
    }

    public Chunk? GetChunk(int chunkX, int chunkZ)
        => _chunks.TryGetValue((chunkX, chunkZ), out var value) ? value : null;

    public void SetChunk(int chunkX, int chunkZ, Chunk chunk)
    {
        var existingChunk = GetChunk(chunkX, chunkZ);

        if (existingChunk != null)
            existingChunk.Sections = chunk.Sections;
        else
            _chunks[(chunkX, chunkZ)] = chunk;
    }
}
