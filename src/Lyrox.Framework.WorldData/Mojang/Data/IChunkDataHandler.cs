using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.WorldData.Mojang.Data
{
    internal interface IChunkDataHandler
    {
        Chunk HandleChunkData(byte[] data);
    }
}
