namespace Lyrox.Framework.CodeGeneration.Test;

using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

// Parsing Idea

// Simple Packet
[AutoSerialized]
public record Handshake([VarInt] int HandshakeId);

// Complex Packet
[AutoSerialized]
public record ConnectionEstablished([Optional]int Id, [LengthPrefixed]string Name, bool Active);

//[AutoSerialized]
//public record PlayerList([Optional][LengthPrefixed]string[] PlayerNames);

// Custom Packet
[CustomSerialized<Login, LoginParser>]
public record Login(string Name);

public class LoginParser : IPacketSerializer<Login>
{
    public Login Deserialize(IMojangBinaryReader reader)
        => new(reader.ReadStringWithVarIntPrefix());

    public void Serialize(IMojangBinaryWriter writer, Login packet)
        => writer.WriteStringWithVarIntPrefix(packet.Name);
}
