using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Core.Data.Abstraction;
public interface IPacketSerializer
{
    object DeserializePacket(int packetId, IMinecraftBinaryReader reader);
    byte[] SerializePacket(Type packetType, object packet);
    public int? GetOpCode(Type packetType);
}
