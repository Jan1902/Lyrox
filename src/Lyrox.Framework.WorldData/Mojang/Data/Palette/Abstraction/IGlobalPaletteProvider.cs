using Lyrox.Framework.Core.Models.World;

namespace Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;

internal interface IGlobalPaletteProvider
{
    BlockState GetStateFromId(int id);
}
