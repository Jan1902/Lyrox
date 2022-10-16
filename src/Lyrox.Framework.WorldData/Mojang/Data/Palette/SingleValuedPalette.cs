using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette
{
    internal class SingleValuedPalette : IPalette
    {
        private BlockState _defaultState;
        private readonly IGlobalPaletteProvider _globalPaletteProvider;

        public SingleValuedPalette(IGlobalPaletteProvider globalPaletteProvider)
            => _globalPaletteProvider = globalPaletteProvider;

        public int GetBitsPerBlock() => 0;

        public BlockState? GetStateForId(int id) => _defaultState;

        public void Read(MojangBinaryReader reader)
            => _defaultState = _globalPaletteProvider.GetStateFromId(reader.ReadVarInt());
    }
}
