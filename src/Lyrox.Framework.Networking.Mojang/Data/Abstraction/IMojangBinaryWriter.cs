namespace Lyrox.Framework.Networking.Mojang.Data.Abstraction;

public interface IMojangBinaryWriter
{
    Stream UnderlyingStream { get; }
    void WriteBool(bool value);
    void WriteByte(byte value);
    void WriteBytes(byte[] bytes);
    void WriteDouble(double value);
    void WriteFloat(float value);
    void WriteInt(int value);
    void WriteLong(long value);
    void WriteShort(short value);
    void WriteStringWithIntPrefix(string value);
    void WriteStringWithShortPrefix(string value);
    void WriteStringWithVarIntPrefix(string value);
    void WriteUInt(uint value);
    void WriteULong(ulong value);
    void WriteUShort(ushort value);
    void WriteUUID(Guid uuid);
    void WriteVarInt(int value);
}
