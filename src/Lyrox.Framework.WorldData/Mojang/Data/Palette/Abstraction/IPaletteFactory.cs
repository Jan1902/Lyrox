namespace Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction
{
    internal interface IPaletteFactory
    {
        IPalette CreatePalette(int bitsPerBlock, bool isBiome);
    }
}
