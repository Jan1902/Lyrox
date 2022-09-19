using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.WorldData
{
    public class WorldDataManager : IWorldDataManager
    {
        private readonly Dictionary<(int X, int Z), Chunk> _chunks;

        public WorldDataManager()
            => _chunks = new();

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
}
