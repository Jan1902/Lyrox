namespace Lyrox.Framework.Core.Models.World;

public class ChunkSection
{
    public BlockState?[,,] BlockStates { get; }

    public ChunkSection(BlockState[,,] blockStates)
        => BlockStates = blockStates;
}
