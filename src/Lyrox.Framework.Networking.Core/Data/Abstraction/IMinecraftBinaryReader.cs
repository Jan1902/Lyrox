using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking.Mojang.Data.Abstraction;

public interface IMinecraftBinaryReader : IDisposable
{
    bool ReadBool();
    byte ReadByte();
    byte[] ReadBytes(int count);
    double ReadDouble();
    float ReadFloat();
    int ReadInt();
    long ReadLong();
    short ReadShort();
    string ReadStringWithIntPrefix();
    string ReadStringWithLength(int length);
    string ReadStringWithShortPrefix();
    string ReadStringWithVarIntPrefix();
    uint ReadUInt();
    ulong ReadULong();
    ushort ReadUShort();
    Guid ReadUUID();
    int ReadVarInt();
    Vector3i ReadPosition();
}
