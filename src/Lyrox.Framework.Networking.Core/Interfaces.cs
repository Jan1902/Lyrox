using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.CodeGeneration.Shared;

public interface IPacketSerializer<TPacket> where TPacket : IPacket
{
    TPacket Deserialize(IMinecraftBinaryReader reader);
    void Serialize(IMinecraftBinaryWriter writer, TPacket packet);
}

[AttributeUsage(AttributeTargets.Class)]
public class AutoSerializedPacketAttribute : Attribute
{
    public int PacketID { get; init; }

    public AutoSerializedPacketAttribute(int packetId)
        => PacketID = packetId;
}

[AttributeUsage(AttributeTargets.Class)]
public class CustomSerializedPacketAttribute<TPacket, TParser> : Attribute
    where TPacket : IPacket
    where TParser : IPacketSerializer<TPacket>
{
    public int PacketID { get; init; }

    public CustomSerializedPacketAttribute(int packetId)
        => PacketID = packetId;
}
