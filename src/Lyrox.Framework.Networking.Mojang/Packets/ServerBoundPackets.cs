using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.Packets;

[AutoSerializedPacket(0x00)]
public record Handshake(int ProtocolVersion, string ServerAddress, ushort ServerPort, ProtocolState NextState) : IPacket;

[AutoSerializedPacket(0x20)]
public record KeepAliveSB(long KeepAliveID) : IPacket;

[CustomSerializedPacket<LoginStart, LoginStartSerializer>(0x00)]
public record LoginStart(string Name, bool HasSigData, long TimeStamp, byte[] PublicKey, byte[] Signature) : IPacket;

public class LoginStartSerializer : IPacketSerializer<LoginStart>
{
    public LoginStart Deserialize(IMinecraftBinaryReader reader) => throw new NotImplementedException();

    public void Serialize(IMinecraftBinaryWriter writer, LoginStart packet)
    {
        writer.WriteStringWithVarIntPrefix(packet.Name);
        writer.WriteBool(packet.HasSigData);

        if (packet.HasSigData)
        {
            writer.WriteLong(packet.TimeStamp);
            writer.WriteVarInt(packet.PublicKey.Length);
            writer.WriteBytes(packet.PublicKey);
            writer.WriteVarInt(packet.Signature.Length);
            writer.WriteBytes(packet.Signature);
        }

        writer.WriteBool(false);
    }
}
