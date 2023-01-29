namespace Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;

internal interface IPaletteFactory
{
    IPalette CreatePalette(int bitsPerBlock, bool isBiome);
}
