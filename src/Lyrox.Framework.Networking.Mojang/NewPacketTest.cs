using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Data.Types;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking.Mojang;

[AttributeUsage(AttributeTargets.Parameter)]
internal class VarIntAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter)]
internal class PositionAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter)]
internal class LengthPrefixedAttribute : Attribute
{
    public LengthPrefix LengthPrefixType { get; private set; }

    public LengthPrefixedAttribute(LengthPrefix lengthPrefixType)
        => LengthPrefixType = lengthPrefixType;
}

[AttributeUsage(AttributeTargets.Parameter)]
internal class OptionalAttribute : Attribute { }

internal interface IAutoParsedPacket { }
internal interface ICustomParsedPacket { void Parse(Stream stream); }

internal enum LengthPrefix { VarInt, Int, UInt }

internal record BlockUpdate([Position] Vector3i Location, [VarInt] int ID) : IAutoParsedPacket;
internal record KeepAlive(long KeepAliveID) : IAutoParsedPacket;
internal record LoginSuccess(Guid UUID, string Username) : IAutoParsedPacket;

internal record PlayerChatMessage(
    [Optional][LengthPrefixed(LengthPrefix.VarInt)] byte[] MessageSignature,
    Guid SenderUUID,
    [LengthPrefixed(LengthPrefix.VarInt)] byte[] HeaderSignature,
    string PlainMessage,
    [Optional] string FormattedMessage,
    DateTime Timestamp,
    long Salt) : IAutoParsedPacket;

internal class Deserializer
{
    public static TPacket Deserialize<TPacket>(IMojangBinaryReader reader) where TPacket : IAutoParsedPacket
    {
        var parameters = typeof(TPacket).GetConstructors().First().GetParameters();
        var paramList = new List<object>();

        foreach (var param in parameters)
        {
            var attributes = new Queue<object>(param.GetCustomAttributes(false));

            paramList.Add(DeserializeParameter(reader, param.ParameterType, attributes)!);
        }

        return (TPacket)Activator.CreateInstance(typeof(TPacket), paramList.ToArray())!;
    }

    private static object? DeserializeParameter(IMojangBinaryReader reader, Type type, Queue<object> attributes)
    {
        while (attributes.Any())
        {
            switch (attributes.Peek())
            {
                case OptionalAttribute:
                    _ = attributes.Dequeue();
                    if (reader.ReadBool())
                        continue;
                    return null;
                case VarIntAttribute:
                    return ReadPrimitive(reader, typeof(VarInt));
                case PositionAttribute:
                    return ReadPrimitive(reader, typeof(Position));
                case LengthPrefixedAttribute lengthPrefix:
                    _ = attributes.Dequeue();

                    var length = ReadLengthPrefix(reader, lengthPrefix.LengthPrefixType);
                    if (type == typeof(string)
                        || type == typeof(byte[]))
                        return ReadPrimitive(reader, type, length);

                    var array = Array.CreateInstance(type.GetElementType()!, length);
                    for (var i = 0; i < length; i++)
                        array.SetValue(DeserializeParameter(reader, type.GetElementType()!, attributes), i);
                    return array;

                default:
                    return ReadPrimitive(reader, type);
            }
        }

        return ReadPrimitive(reader, type);
    }

    private static int ReadLengthPrefix(IMojangBinaryReader reader, LengthPrefix prefixType) => prefixType switch
    {
        LengthPrefix.VarInt => reader.ReadVarInt(),
        LengthPrefix.Int => reader.ReadInt(),
        LengthPrefix.UInt => (int)reader.ReadUInt(),
        _ => throw new NotSupportedException()
    };

    private static object ReadPrimitive(IMojangBinaryReader reader, Type type, int? length = null) => type switch
    {
        Type t when t == typeof(VarInt) => reader.ReadVarInt(),
        Type t when t == typeof(Position) => reader.ReadPosition(),
        Type t when t == typeof(int) => reader.ReadInt(),
        Type t when t == typeof(long) => reader.ReadLong(),
        Type t when t == typeof(DateTime) => new DateTime(reader.ReadLong()),
        Type t when t == typeof(string) => length != null ? reader.ReadStringWithLength(length ?? 0) : reader.ReadStringWithVarIntPrefix(),
        Type t when t == typeof(Guid) => reader.ReadUUID(),
        Type t when t == typeof(byte[]) => reader.ReadBytes(length ?? 0),
        _ => throw new NotSupportedException("Given Data Type can not be parsed"),
    };
}
