using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction
{
    internal interface IGlobalPaletteProvider
    {
        int GetIdFromState(BlockState state);
        BlockState GetStateFromId(int id);
    }
}
