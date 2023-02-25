using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.World.Mojang.Data;

internal interface IChunkDataHandler
{
    Chunk HandleChunkData(byte[] data);
}
