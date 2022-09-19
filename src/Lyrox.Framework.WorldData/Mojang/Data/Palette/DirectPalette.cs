using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette
{
    internal class DirectPalette : IPalette
    {
        private IGlobalPaletteProvider _globalPaletteProvider;

        public DirectPalette(IGlobalPaletteProvider globalPaletteProvider)
            => _globalPaletteProvider = globalPaletteProvider;

        public int GetBitsPerBlock()
            => (int)Math.Ceiling(Math.Log2(15));

        public BlockState? GetStateForId(int id)
            => _globalPaletteProvider.GetStateFromId(id);

        public void Read(MojangBinaryReader reader) { }
    }
}
