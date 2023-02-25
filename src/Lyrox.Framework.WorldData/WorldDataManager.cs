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
            var chunkX = blockX / 16;
            var chunkY = blockY / 16 + 4;
            var chunkZ = blockZ / 16;

        var xOffset = blockX % 16;
        var yOffset = blockY % 16;
        var zOffset = blockZ % 16;

        return _chunks.ContainsKey((chunkX, chunkZ)) ?
            _chunks[(chunkX, chunkZ)]
            .Sections[chunkY]
            .BlockStates[xOffset, yOffset, zOffset]
            : null;
    }

        public void SetBlock(int blockX, int blockY, int blockZ, BlockState blockState)
        {
            var chunkX = blockX / 16;
            var chunkY = blockY / 16 + 4;
            var chunkZ = blockZ / 16;

        var xOffset = blockX % 16;
        var yOffset = blockY % 16;
        var zOffset = blockZ % 16;

        _chunks[(chunkX, chunkZ)]
            .Sections[chunkY]
            .BlockStates[xOffset, yOffset, zOffset] = blockState;
    }

    public Chunk? GetChunk(int chunkX, int chunkZ)
        => _chunks.ContainsKey((chunkX, chunkZ)) ? _chunks[(chunkX, chunkZ)] : null;

    public void SetChunk(int chunkX, int chunkZ, Chunk chunk)
    {
        var existingChunk = GetChunk(chunkX, chunkZ);

        if (existingChunk != null)
            existingChunk.Sections = chunk.Sections;
        else
            _chunks[(chunkX, chunkZ)] = chunk;
    }
}
