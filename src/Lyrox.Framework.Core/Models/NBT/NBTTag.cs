namespace Lyrox.Framework.Core.Models.NBT
{
    public abstract class NBTTag
    {
        public string? Name { get; set; }
    }

    public class CompoundTag : NBTTag
    {
        public IEnumerable<NBTTag> Children { get; set; }
    }

    public class IntTag : NBTTag
    {
        public int Value { get; set; }
    }

    public class ByteTag : NBTTag
    {
        public byte Value { get; set; }
    }

    public class ShortTag : NBTTag
    {
        public short Value { get; set; }
    }

    public class LongTag : NBTTag
    {
        public long Value { get; set; }
    }

    public class FloatTag : NBTTag
    {
        public float Value { get; set; }
    }

    public class DoubleTag : NBTTag
    {
        public double Value { get; set; }
    }

    public class StringTag : NBTTag
    {
        public string Value { get; set; }
    }

    public class ListTag : NBTTag
    {
        public Type TagType { get; set; }
        public IEnumerable<NBTTag> Items { get; set; }
    }

    public class IntArrayTag : NBTTag
    {
        public int[] Items { get; set; }
    }

    public class LongArrayTag : NBTTag
    {
        public long[] Items { get; set; }
    }
}
