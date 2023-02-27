using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.Networking.Mojang.Data.Types;
using Lyrox.Framework.Networking.Mojang.Packets.Base;
using Lyrox.Framework.Shared.Types;

internal class VarIntAttribute : Attribute { }
internal class PositionAttribute : Attribute { }
internal class LengthPrefixedAttribute : Attribute { }
internal class OptionalAttribute : Attribute { }

internal record BlockUpdate([Position] Vector3i Location, [VarInt] int ID);
internal record KeepAlive(long KeepAliveID);
internal record LoginSuccess(Guid UUID, string Username);

internal record PlayerChatMessage(
    [Optional][LengthPrefixed] byte[] MessageSignature,
    Guid SenderUUID,
    [LengthPrefixed] byte[] HeaderSignature,
    string PlainMessage,
    [Optional] string FormattedMessage,
    DateTime Timestamp,
    long Salt);


internal class Deserializer
{
    public TPacket Deserialize<TPacket>(MojangBinaryReader reader) where TPacket : MojangClientBoundPacketBase
    {
        var parameters = typeof(TPacket).GetConstructors().First().GetParameters();
        var paramList = new List<object>();

        foreach (var param in parameters)
        {
            var attributes = new Queue<object>(param.GetCustomAttributes(false));

            while(attributes.Any())
            {
                switch (attributes.Dequeue())
                {
                    case OptionalAttribute:
                        if (reader.ReadBool())
                            goto case null;
                        break;

                    case null:
                        paramList.Add(ReadType(reader, param.ParameterType));
                        break;
                }
            }
        }

        return (TPacket)Activator.CreateInstance(typeof(TPacket), paramList)!;
    }

    private object DeserializeValue()
    {
        
    }

    private object ReadType(MojangBinaryReader reader, Type type)
    {
        switch (type.GetCustomAttributes(false).Last())
        {
            case VarIntAttribute:
                return ReadPrimitive(reader, typeof(VarInt));
            case PositionAttribute:
                return ReadPrimitive(reader, typeof(Position));
            case LengthPrefixedAttribute:
                var length = reader.ReadVarInt();
                var array = Array.CreateInstance(type.GetElementType()!, length);
                for (int i = 0; i < length; i++)
                    array.SetValue(ReadPrimitive(reader, type.GetElementType()!), i);
                return array;

            default:
                return ReadPrimitive(reader, type);
        }
    }

    private object ReadPrimitive(MojangBinaryReader reader, Type type)
    {
        switch (type)
        {
            case Type t when t == typeof(VarInt):
                return reader.ReadVarInt();
            case Type t when t == typeof(Position):
                return reader.ReadPosition();
            case Type t when t == typeof(int):
                return reader.ReadInt();
            case Type t when t == typeof(DateTime):
                return new DateTime(reader.ReadLong());
            case Type t when t == typeof(string):
                return reader.ReadStringWithVarIntPrefix();

            default:
                throw new Exception("Given Data Type can not be parsed");
        }
    }
}