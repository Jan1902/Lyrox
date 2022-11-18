namespace Lyrox.Framework.Core.Models.NBT;

public abstract class NBTTag
{
    public string? Name { get; set; }

    protected NBTTag(string? name) => Name = name;
}

public class CompoundTag : NBTTag
{
    public IEnumerable<NBTTag> Children { get; set; }

    public CompoundTag(IEnumerable<NBTTag> children, string? name = null) : base(name)
        => Children = children;

    public CompoundTag(string? name = null) : base(name)
        => Children = new List<NBTTag>();
}

public class IntTag : NBTTag
{
    public int? Value { get; set; }

    public IntTag(int value, string? name = null) : base(name)
        => Value = value;

    public IntTag(string? name = null) : base(name) { }
}

public class ByteTag : NBTTag
{
    public byte? Value { get; set; }

    public ByteTag(byte value, string? name = null) : base(name)
        => Value = value;

    public ByteTag(string? name = null) : base(name) { }
}

public class ShortTag : NBTTag
{
    public short? Value { get; set; }

    public ShortTag(short value, string? name = null) : base(name)
        => Value = value;

    public ShortTag(string? name = null) : base(name) { }
}

public class LongTag : NBTTag
{
    public long? Value { get; set; }

    public LongTag(long value, string? name = null) : base(name)
        => Value = value;

    public LongTag(string? name = null) : base(name) { }
}

public class FloatTag : NBTTag
{
    public float? Value { get; set; }

    public FloatTag(float value, string? name = null) : base(name)
        => Value = value;

    public FloatTag(string? name = null) : base(name) { }
}

public class DoubleTag : NBTTag
{
    public double? Value { get; set; }

    public DoubleTag(double value, string? name = null) : base(name)
        => Value = value;

    public DoubleTag(string? name = null) : base(name) { }
}

public class StringTag : NBTTag
{
    public string? Value { get; set; }

    public StringTag(string value, string? name = null) : base(name)
        => Value = value;

    public StringTag(string? name = null) : base(name) { }
}

public class ListTag : NBTTag
{
    public Type? TagType { get; set; }
    public IEnumerable<NBTTag> Items { get; set; }

    public ListTag(Type tagType, IEnumerable<NBTTag> items, string? name = null) : base(name)
    {
        TagType = tagType;
        Items = items;
    }

    public ListTag(Type tagType, string? name = null) : base(name)
    {
        TagType = tagType;
        Items = new List<NBTTag>();
    }

    public ListTag(string? name = null) : base(name)
        => Items = new List<NBTTag>();
}

public class IntArrayTag : NBTTag
{
    public int[] Items { get; set; }

    public IntArrayTag(int[] items, string? name = null) : base(name)
        => Items = items;

    public IntArrayTag(string? name = null) : base(name)
        => Items = Array.Empty<int>();
}

public class LongArrayTag : NBTTag
{
    public long[] Items { get; set; }

    public LongArrayTag(long[] items, string? name = null) : base(name)
        => Items = items;

    public LongArrayTag(string? name = null) : base(name)
        => Items = Array.Empty<long>();
}

public class EndTag : NBTTag
{
    public EndTag() : base(null) { }
}
