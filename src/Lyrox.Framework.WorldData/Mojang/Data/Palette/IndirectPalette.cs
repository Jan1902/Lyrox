using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette
{
    internal class IndirectPalette : IPalette
    {
        private readonly Dictionary<int, BlockState> _idToState;
        private readonly int _bitsPerBlock;

        private readonly IGlobalPaletteProvider _globalPaletteProvider;

        public IndirectPalette(int bitsPerBlock, IGlobalPaletteProvider globalPaletteProvider)
        {
            _idToState = new();

            _bitsPerBlock = bitsPerBlock;
            _globalPaletteProvider = globalPaletteProvider;
        }

        public BlockState? GetStateForId(int id)
            => _idToState.ContainsKey(id) ? _idToState[id] : null;

        public int GetBitsPerBlock()
            => _bitsPerBlock;

        public void Read(MojangBinaryReader reader)
        {
            var length = reader.ReadVarInt();

            for (var id = 0; id < length; id++)
            {
                var stateId = reader.ReadVarInt();
                var state = _globalPaletteProvider.GetStateFromId(stateId);

                _idToState[id] = state;
            }
        }
    }
}
