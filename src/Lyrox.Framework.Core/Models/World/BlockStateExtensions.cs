namespace Lyrox.Framework.Core.Models.World;

public static class BlockStateExtensions
{
    private static readonly string[] NonSolidBlocks = new[]
    {
        "air",
        "grass"
    };

    public static bool IsSolid(this BlockState blockState)
        => !NonSolidBlocks.Contains(blockState.BlockName.Split(':').Last().ToLower());
}
