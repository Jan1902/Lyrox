namespace Lyrox.Core.Models.World
{
    public class ChunkSection
    {
        public BlockState[,,] BlockStates { get; private set; } = new BlockState[16, 16, 16];
        public int[,,] BlockLights { get; private set; } = new int[16, 16, 16];
        public int[,,] SkyLights { get; private set; } = new int[16, 16, 16];
    }
}
