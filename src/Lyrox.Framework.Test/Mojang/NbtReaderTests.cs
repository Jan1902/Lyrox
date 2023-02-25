using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.Data;

namespace Lyrox.Framework.Test.Mojang;

public class NbtReaderTests
{
    [Test]
    public void TestSmall()
    {
        using var stream = new MemoryStream(Resources.smalltest);
        using var reader = new MojangBinaryReader(stream);

        var nbtRaw = MojangNBTReader.ParseNBT(reader);

        nbtRaw.Should().NotBeNull().And.BeOfType<CompoundTag>();

        var compound = nbtRaw as CompoundTag;

        var expected = new CompoundTag(new[] { new StringTag("Bananrama", "name") }, "hello world");
        compound.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TestBig()
    {
        using var stream = new MemoryStream(Resources.bigtest);
        using var reader = new MojangBinaryReader(stream);

        var nbtRaw = MojangNBTReader.ParseNBT(reader);

        nbtRaw.Should().NotBeNull().And.BeOfType<CompoundTag>();

        var compound = nbtRaw as CompoundTag;

        var expected = new CompoundTag(new NBTTag[]
        {
            new CompoundTag(new NBTTag[]
            {
                new CompoundTag(new NBTTag[]
                {
                    new StringTag("Eggbert", "name"),
                    new FloatTag(0.5f, "value")
                }, "egg"),
                new CompoundTag(new NBTTag[]
                {
                    new StringTag("Hampus", "name"),
                    new FloatTag(0.75f, "value")
                }, "ham")
            }, "nested compound test"),
            new IntTag(2147483647, "intTest"),
            new ByteTag(127, "byteTest"),
            new StringTag("HELLO WORLD THIS IS A TEST STRING \xc5\xc4\xd6!", "stringTest"),
            new ListTag(typeof(long), new NBTTag[]
            {
                new LongTag(11),
                new LongTag(12),
                new LongTag(13),
                new LongTag(14),
                new LongTag(15),
            }, "listTest (long)"),
            new DoubleTag(0.49312871321823148d, "doubleTest"),
            new FloatTag(0.49823147058486938f, "floatTest"),
            new LongTag(9223372036854775807L, "longTest"),
            new ListTag(typeof(CompoundTag), new NBTTag[]
            {
                new CompoundTag(new NBTTag[]
                {
                    new LongTag(1264099775885L, "created-on"),
                    new StringTag("Compound tag #0", "name")
                }),
                new CompoundTag(new NBTTag[]
                {
                    new LongTag(1264099775885L, "created-on"),
                    new StringTag("Compound tag #1", "name")
                })
            }, "listTest (compound)"),
            new ByteArrayTag(Enumerable.Range(1, 1000).Select(n => (byte)((n * n * 255 + n * 7) % 100)).ToArray(),
                "byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))"),
            new ShortTag(32767, "shortTest")
        }, "Level");

        compound.Should().BeEquivalentTo(expected);
    }
}
