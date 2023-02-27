using System.Text;
using BitConverter;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Data.Types;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking.Mojang.Data;

public class MojangBinaryWriter : IMojangBinaryWriter, IDisposable
{
    private readonly Stream _stream;
    private readonly EndianBitConverter _bitConverter;

    public MojangBinaryWriter(Stream stream)
    {
        _stream = stream;
        _bitConverter = EndianBitConverter.BigEndian;
    }

    public Stream UnderlyingStream => _stream;

    public void ResetStreamPosition()
        => _stream.Seek(0, SeekOrigin.Begin);

    public void WriteVarInt(int value)
        => _stream.WriteVarInt(value);

    public void WritePosition(Vector3i value)
        => _stream.WritePosition(value);

    public void WriteByte(byte value)
        => _stream.WriteByte(value);

    public void WriteBytes(byte[] bytes)
        => _stream.Write(bytes);

    public void WriteBool(bool value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteUShort(ushort value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteShort(short value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteUInt(uint value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteInt(int value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteULong(ulong value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteLong(long value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteFloat(float value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteDouble(double value)
        => WriteBytes(_bitConverter.GetBytes(value));

    public void WriteStringWithVarIntPrefix(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteVarInt(bytes.Length);
        WriteBytes(bytes);
    }

    public void WriteStringWithIntPrefix(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt(bytes.Length);
        WriteBytes(bytes);
    }

    public void WriteStringWithShortPrefix(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteUShort((ushort)bytes.Length);
        WriteBytes(bytes);
    }

    public void WriteUUID(Guid uuid)
    {
        var data = uuid.ToByteArray();

        _bitConverter.ToUInt64(data, 8);
        _bitConverter.ToUInt64(data, 0);

        WriteBytes(data);
    }

    public void Dispose()
        => _stream.Dispose();
}
