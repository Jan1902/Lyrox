namespace Lyrox.Framework.CodeGeneration.Test;

using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

// Parsing Idea

// Simple Packet
[AutoParsed]
public record Handshake([VarInt] int HandshakeId);

// Complex Packet
[AutoParsed]
public record Test([Optional]int Id, [LengthPrefixed]string Name, bool Active);

[AutoParsed]
public record PlayerList([Optional][LengthPrefixed]string[] PlayerNames);

// Custom Packet
[CustomParsed<Login, LoginParser>]
public record Login(string Name);

public class LoginParser : IPacketParser<Login>
{
    public Login Deserialize(IMojangBinaryReader reader)
        => new(reader.ReadStringWithVarIntPrefix());

    public void Serialize(IMojangBinaryWriter writer, Login packet)
        => writer.WriteStringWithVarIntPrefix(packet.Name);
}
