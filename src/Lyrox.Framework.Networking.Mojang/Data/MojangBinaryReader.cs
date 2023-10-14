using System.IO;
using System.Text;
using BitConverter;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Data.Types;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking.Mojang.Data;

public class MojangBinaryReader : IMinecraftBinaryReader
{
    private readonly Stream _stream;
    private readonly EndianBitConverter _bitConverter;

    public MojangBinaryReader(Stream stream)
    {
        _stream = stream;
        _bitConverter = EndianBitConverter.BigEndian;
    }

    public int ReadVarInt()
    {
        var numRead = 0;
        var result = 0;
        byte read;

        do
        {
            read = ReadByte();
            var value = read & 0x7f;
            result |= value << 7 * numRead;

            numRead++;
            if (numRead > 5)
                throw new Exception("VarInt is too big");
        }
        while ((read & 0x80) != 0);

        return result;
    }

    public Vector3i ReadPosition()
    {
        var value = ReadLong();

        var x = value >> 38;
        var y = value << 52 >> 52;
        var z = value << 26 >> 38;

        return new Vector3i((int)x, (int)y, (int)z);
    }

    public byte ReadByte()
        => (byte)_stream.ReadByte();

    public byte[] ReadBytes(int count)
    {
        var buffer = new byte[count];
        _stream.Read(buffer, 0, count);
        return buffer;
    }

    public bool ReadBool()
        => _bitConverter.ToBoolean(ReadBytes(sizeof(bool)), 0);

    public ushort ReadUShort()
        => _bitConverter.ToUInt16(ReadBytes(sizeof(ushort)), 0);

    public short ReadShort()
        => _bitConverter.ToInt16(ReadBytes(sizeof(short)), 0);

    public int ReadInt()
        => _bitConverter.ToInt32(ReadBytes(sizeof(int)), 0);

    public uint ReadUInt()
        => _bitConverter.ToUInt32(ReadBytes(sizeof(uint)), 0);

    public ulong ReadULong()
        => _bitConverter.ToUInt64(ReadBytes(sizeof(ulong)), 0);

    public long ReadLong()
        => _bitConverter.ToInt64(ReadBytes(sizeof(long)), 0);

    public float ReadFloat()
        => _bitConverter.ToSingle(ReadBytes(sizeof(float)), 0);

    public double ReadDouble()
        => _bitConverter.ToDouble(ReadBytes(sizeof(double)), 0);

    public string ReadStringWithVarIntPrefix()
        => Encoding.UTF8.GetString(ReadBytes(ReadVarInt()));

    public string ReadStringWithIntPrefix()
        => Encoding.UTF8.GetString(ReadBytes(ReadInt()));

    public string ReadStringWithShortPrefix()
        => Encoding.UTF8.GetString(ReadBytes(ReadUShort()));

    public string ReadStringWithLength(int length)
        => Encoding.UTF8.GetString(ReadBytes(length));

    public Guid ReadUUID()
    {
        var data = new byte[16];
        var dataA = ReadBytes(8);
        var dataB = ReadBytes(8);

        Array.Copy(dataA, 0, data, 0, 8);
        Array.Copy(dataB, 0, data, 8, 8);

        return new Guid(data);
    }

    public void Dispose()
        => _stream.Dispose();
}
