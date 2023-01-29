namespace Lyrox.Framework.Core.Models.World;

public class Chunk
{
    public ChunkSection[] Sections { get; set; }

    public Chunk(ChunkSection[] sections)
        => Sections = sections;
}
