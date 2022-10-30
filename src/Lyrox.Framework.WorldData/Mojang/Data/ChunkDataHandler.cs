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
            using var stream = new MemoryStream(data);
            var reader = new MojangBinaryReader(stream);

            var sections = new ChunkSection[24];
            for (var i = 0; i < 24; i++)
            {
                //Console.WriteLine(i);
                sections[i] = HandleChunkSection(reader);
            }

            return new Chunk(sections);
        }

        private ChunkSection HandleChunkSection(MojangBinaryReader reader)
        {
            _ = reader.ReadShort();

            var bitsPerEntry = reader.ReadByte();
            var palette = _paletteFactory.CreatePalette(bitsPerEntry, false);
            palette.Read(reader);
            bitsPerEntry = (byte)palette.GetBitsPerBlock();

            var states = new BlockState[16, 16, 16];

            if (bitsPerEntry == 0)
            {
                var state = palette.GetStateForId(0);

                _ = reader.ReadVarInt();

                for (var y = 0; y < 16; y++)
                    for (var z = 0; z < 16; z++)
                        for (var x = 0; x < 16; x++)
                            states[x, y, z] = state;

                DiscardBiomeData();

                return new ChunkSection(states);
            }

            var individualValueMask = (uint)((1 << bitsPerEntry) - 1);

            var data = new ulong[reader.ReadVarInt()];
            for (var i = 0; i < data.Length; i++)
                data[i] = reader.ReadULong();

            var offset = 64 % bitsPerEntry;
            var entriesPerLong = 64 / bitsPerEntry;

            var lastOffset = 0;

            for (var y = 0; y < 16; y++)
            {
                for (var z = 0; z < 16; z++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var blockNumber = (((y * 16) + z) * 16) + x;
                        var longIndex = blockNumber / entriesPerLong;
                        var individualOffset = offset + (blockNumber % entriesPerLong) * bitsPerEntry;
                        individualOffset = (blockNumber * bitsPerEntry) % 64;

                        var id = (uint)(data[longIndex] >> individualOffset);
                        id &= individualValueMask;

                        var state = palette.GetStateForId((int)id);

                        //if (state is null)
                        //    Console.WriteLine();
                        states[x, y, z] = state;
                        lastOffset = individualOffset;
                    }
                }
            }

            DiscardBiomeData();

            return new ChunkSection(states);

            void DiscardBiomeData()
            {
                var bitsPerEntry = reader.ReadByte();
                _paletteFactory.CreatePalette(bitsPerEntry, true).Read(reader);

                var length = reader.ReadVarInt();
                for (var i = 0; i < length; i++)
                    reader.ReadLong();
            }
        }
    }
}
