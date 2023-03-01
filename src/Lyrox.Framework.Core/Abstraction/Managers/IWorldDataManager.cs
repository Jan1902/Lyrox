using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface IWorldDataManager
{
    Chunk? GetChunk(int chunkX, int chunkZ);
    BlockState? GetBlock(int blockX, int blockY, int blockZ);
    BlockState? GetBlock(Vector3i blockPosition)
        => GetBlock(blockPosition.X, blockPosition.Y, blockPosition.Z);
}
