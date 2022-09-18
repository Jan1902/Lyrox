namespace Lyrox.Framework.Core.Models.World
{
    public class ChunkSection
    {
        public BlockState[,,] BlockStates { get; } = new BlockState[16, 16, 16];
    }
}
