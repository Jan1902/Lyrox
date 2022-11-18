using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Mojang
{
    public class MojangNBTReader
    {
        private static readonly Dictionary<int, Type> _tagTypeIds = new()
        {
            { 0, typeof(EndTag) },
            { 1, typeof(ByteTag) },
            { 2, typeof(ShortTag) },
            { 3, typeof(IntTag) },
            { 4, typeof(LongTag) },
            { 5, typeof(FloatTag) },
            { 6, typeof(DoubleTag) },
            { 7, typeof(ByteArrayTag) },
            { 8, typeof(StringTag) },
            { 9, typeof(ListTag) },
            { 10, typeof(CompoundTag) },
            { 11, typeof(IntArrayTag) },
            { 12, typeof(LongArrayTag) }
        };

        public static NBTTag ParseNBT(IMojangBinaryReader reader)
            => ParseRecursive(reader, null);

        private static NBTTag ParseRecursive(IMojangBinaryReader reader, NBTTag? parent, int? givenTypeId = null)
        {
            var typeId = givenTypeId ?? reader.ReadByte();
            switch (typeId)
            {
                case 0:
                    return new EndTag();
                case 1:
                    return new ByteTag() { Name = GetName(), Value = reader.ReadByte() };
                case 2:
                    return new ShortTag() { Name = GetName(), Value = reader.ReadShort() };
                case 3:
                    return new IntTag() { Name = GetName(), Value = reader.ReadInt() };
                case 4:
                    return new LongTag() { Name = GetName(), Value = reader.ReadLong() };
                case 5:
                    return new FloatTag() { Name = GetName(), Value = reader.ReadFloat() };
                case 6:
                    return new DoubleTag() { Name = GetName(), Value = reader.ReadDouble() };
                case 7:
                    return new ByteArrayTag() { Name = GetName(), Value = reader.ReadBytes(reader.ReadInt()) };
                case 8:
                    return new StringTag() { Name = GetName(), Value = reader.ReadStringWithShortPrefix() };
                case 9:
                    {
                        var listTag = new ListTag() { Name = GetName() };

                        var listTypeId = reader.ReadByte();
                        if (!_tagTypeIds.ContainsKey(listTypeId))
                            throw new Exception($"Error parsing NBT List Tag with Type ID {listTypeId}");

                        listTag.TagType = _tagTypeIds[listTypeId];

                        var length = reader.ReadInt();
                        var items = new List<NBTTag>();

                        for (var i = 0; i < length; i++)
                            items.Add(ParseRecursive(reader, listTag, listTypeId));

                        listTag.Items = items;
                        return listTag;
                    }
                case 10:
                    {
                        var compoundTag = new CompoundTag(GetName());
                        var children = new List<NBTTag>();
                        var child = ParseRecursive(reader, compoundTag);

                        while (child is not EndTag)
                        {
                            children.Add(child);
                            child = ParseRecursive(reader, compoundTag);
                        }

                        compoundTag.Children = children;
                        return compoundTag;
                    }
                case 11:
                    return new IntArrayTag() { Name = GetName(), Items = Enumerable.Range(1, reader.ReadInt()).Select(_ => reader.ReadInt()).ToArray() };
                case 12:
                    return new LongArrayTag() { Name = GetName(), Items = Enumerable.Range(1, reader.ReadInt()).Select(_ => reader.ReadLong()).ToArray() };
                default:
                    throw new Exception($"Error parsing NBT Tag with Type ID {typeId}!");
            }

            string? GetName()
                => typeId == 10 || parent is CompoundTag ? reader.ReadStringWithShortPrefix() : null;
        }
    }
}
