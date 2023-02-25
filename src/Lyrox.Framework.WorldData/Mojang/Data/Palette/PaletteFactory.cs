using Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.World.Mojang.Data.Palette;

internal class PaletteFactory : IPaletteFactory
{
    private readonly IGlobalPaletteProvider _globalPaletteProvider;

    public PaletteFactory(IGlobalPaletteProvider globalPaletteProvider)
        => _globalPaletteProvider = globalPaletteProvider;

        public IPalette CreatePalette(int bitsPerBlock, bool isBiome)
        {
            if (bitsPerBlock == 0)
                return new SingleValuedPalette(_globalPaletteProvider);
            else if (isBiome && bitsPerBlock <= 3)
                return new IndirectPalette(bitsPerBlock, _globalPaletteProvider);
            else if (bitsPerBlock <= 4)
                return new IndirectPalette(4, _globalPaletteProvider);
            else if (bitsPerBlock <= 8)
                return new IndirectPalette(bitsPerBlock, _globalPaletteProvider);
            else
                return new DirectPalette(_globalPaletteProvider);
        }
    }
}
