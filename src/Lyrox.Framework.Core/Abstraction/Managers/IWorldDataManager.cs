using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface IWorldDataManager
{
    Chunk? GetChunk(int chunkX, int chunkZ);
    void SetChunk(int chunkX, int chunkZ, Chunk chunk);
    void SetBlock(int blockX, int blockY, int blockZ, BlockState blockState);
    void SetBlock(Vector3i blockPosition, BlockState blockState)
        => SetBlock(blockPosition.X, blockPosition.Y, blockPosition.Z, blockState);
    BlockState? GetBlock(int blockX, int blockY, int blockZ);
    BlockState? GetBlock(Vector3i blockPosition)
        => GetBlock(blockPosition.X, blockPosition.Y, blockPosition.Z);
}
