﻿using Lyrox.Framework.Core.Models.NBT;
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
            { 8, typeof(StringTag) },
            { 9, typeof(ListTag) },
            { 10, typeof(CompoundTag) },
            { 11, typeof(IntArrayTag) },
            { 12, typeof(LongArrayTag) }
        };

        public static NBTTag ParseNBT(IMojangBinaryReader reader)
            => (NBTTag)ParseRecursive(reader, null);

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
                case 8:
                    return new StringTag() { Name = GetName(), Value = reader.ReadStringWithShortPrefix() };
                case 9:
                    {
                        var listTypeId = reader.ReadByte();
                        if (!_tagTypeIds.ContainsKey(listTypeId))
                            throw new Exception($"Error parsing NBT List Tag with Type ID {listTypeId}");

                        var listTag = new ListTag() { Name = GetName(), TagType = _tagTypeIds[listTypeId] };
                        var length = reader.ReadInt();
                        var items = new List<NBTTag>();

                        for (var i = 0; i < length; i++)
                            items.Add(ParseRecursive(reader, listTag, listTypeId));

                        listTag.Items = items;
                        return listTag;
                    }
                case 10:
                    {
                        var compoundTag = new CompoundTag { Name = GetName() };
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
                    {
                        var tag = new IntArrayTag() { Name = GetName(), Items = new int[reader.ReadInt()] };
                        for (var i = 0; i < tag.Items.Length; i++)
                            tag.Items[i] = reader.ReadInt();

                        return tag;
                    }
                case 12:
                    {
                        var tag = new LongArrayTag() { Name = GetName(), Items = new long[reader.ReadInt()] };
                        for (var i = 0; i < tag.Items.Length; i++)
                            tag.Items[i] = reader.ReadLong();

                        return tag;
                    }
                default:
                    throw new Exception($"Error parsing NBT Tag with Type ID {typeId}!");
            }

            string? GetName()
                => typeId == 10 || parent is CompoundTag ? reader.ReadStringWithShortPrefix() : null;
        }
    }
}
