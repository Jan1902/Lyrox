using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.WorldData.Mojang.Data
{
    internal class ChunkDataHandler : IChunkDataHandler
    {
        private readonly IPaletteFactory _paletteFactory;

        public ChunkDataHandler(IPaletteFactory paletteFactory)
            => _paletteFactory = paletteFactory;

        public Chunk HandleChunkData(byte[] data)
        {
            var reader = new MojangBinaryReader(new MemoryStream(data));

            var sections = new ChunkSection[16];
            for (var i = 0; i < 16; i++)
                sections[i] = HandleChunkSection(reader);

            return new Chunk(sections);
        }

        private ChunkSection HandleChunkSection(MojangBinaryReader reader)
        {
            var blockCount = reader.ReadShort();

            var bitsPerEntry = reader.ReadByte();
            var palette = _paletteFactory.CreatePalette(bitsPerEntry);
            palette.Read(reader);

            var states = new BlockState[16, 16, 16];

            if (bitsPerEntry == 0)
            {
                var state = palette.GetStateForId(0);

                for (var y = 0; y < 16; y++)
                    for (var z = 0; z < 16; z++)
                        for (var x = 0; x < 16; x++)
                            states[x, y, z] = state;

                // Biome data is irrelevant for now
                _paletteFactory.CreatePalette(reader.ReadByte()).Read(reader);
                for (int i = 0; i < reader.ReadVarInt(); i++)
                    reader.ReadLong();

                return new ChunkSection(states);
            }

            var individualValueMask = (uint)((1 << bitsPerEntry) - 1);

            var data = new long[reader.ReadVarInt()];
            for (var i = 0; i < data.Length; i++)
                data[i] = reader.ReadLong();

            var offset = 64 % bitsPerEntry;
            var entriesPerLong = 64 / bitsPerEntry;

            for (var y = 0; y < 16; y++)
            {
                for (var z = 0; z < 16; z++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var blockNumber = (((y * 16) + z) * 16) + x;
                        var longIndex = blockNumber / entriesPerLong;
                        var individualOffset = offset + (blockNumber % entriesPerLong) * bitsPerEntry;

                        var id = (uint)(data[longIndex] >> individualOffset);
                        id &= individualValueMask;

                        var state = palette.GetStateForId((int)id);
                        states[x, y, z] = state;
                    }
                }
            }

            // Biome data is irrelevant for now
            _paletteFactory.CreatePalette(reader.ReadByte()).Read(reader);
            for (int i = 0; i < reader.ReadVarInt(); i++)
                reader.ReadLong();

            return new ChunkSection(states);
        }
    }
}
