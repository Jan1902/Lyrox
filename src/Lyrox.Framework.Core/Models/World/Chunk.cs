namespace Lyrox.Framework.Core.Models.World
{
    public class Chunk
    {
        public int X { get; private set; }
        public int Z { get; private set; }

        public int[,] Biomes { get; set; } = new int[16, 16];

        public ChunkSection[] Sections = new ChunkSection[16];

        public Chunk(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}
