using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Core;
public interface IPacketSerializer
{
    object DeserializePacket(int packetId, IMojangBinaryReader reader);
    void SerializePacket(Type packetType, object packet, IMojangBinaryWriter writer);
}
