using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.Core.Abstraction
{
    public interface IWorldDataManager
    {
        Chunk? GetChunk(int chunkX, int chunkZ);
        void SetChunk(int chunkX, int chunkZ, Chunk chunk);
    }
}
