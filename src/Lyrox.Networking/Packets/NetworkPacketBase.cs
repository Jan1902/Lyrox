using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitConverter;
using Lyrox.Networking.Types;

namespace Lyrox.Networking.Packets
{
    internal abstract class NetworkPacketBase
    {
        private readonly Stream _stream;
        private readonly EndianBitConverter _bitConverter;

        protected NetworkPacketBase()
        {
            _bitConverter = EndianBitConverter.BigEndian;
            _stream = new MemoryStream();
        }

        protected NetworkPacketBase(int opCode) : this()
            => _stream.WriteVarInt(opCode);

        protected void AddVarInt(int value)
            => _stream.WriteVarInt(value);

        protected int GetVarInt()
            => _stream.ReadVarInt();

        public void AddBytes(byte[] bytes)
            => _stream.Write(bytes);

        protected byte[] GetBytes(int count)
        {
            var buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            return buffer;
        }

        protected byte[] GetBytes()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            var data = new byte[_stream.Length];
            _stream.Read(data, 0, (int)_stream.Length);
            return data;
        }

        protected void AddUShort(ushort value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected ushort GetUShort()
            => _bitConverter.ToUInt16(GetBytes(sizeof(ushort)), 0);

        protected void AddShort(short value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected short GetShort()
            => _bitConverter.ToInt16(GetBytes(sizeof(short)), 0);

        protected void AddString(string value)
        {
            AddVarInt(Encoding.UTF8.GetBytes(value).Length);
            AddBytes(Encoding.UTF8.GetBytes(value));
        }

        protected string GetString()
            => Encoding.UTF8.GetString(GetBytes(GetVarInt()));

        protected string GetString(int length)
            => Encoding.UTF8.GetString(GetBytes(length));

        protected void AddByte(byte value)
            => _stream.WriteByte(value);

        protected byte GetByte()
            => (byte)_stream.ReadByte();

        protected void AddBool(bool value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected bool GetBool()
            => _bitConverter.ToBoolean(GetBytes(sizeof(bool)), 0);

        protected void AddDouble(double value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected double GetDouble()
            => _bitConverter.ToDouble(GetBytes(sizeof(double)), 0);

        protected void AddFloat(float value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected float GetFloat()
            => _bitConverter.ToSingle(GetBytes(sizeof(float)), 0);

        protected void AddLong(long value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected long GetLong()
            => _bitConverter.ToInt64(GetBytes(sizeof(long)), 0);

        protected void AddULong(ulong value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected ulong GetULong()
            => _bitConverter.ToUInt64(GetBytes(sizeof(ulong)), 0);

        protected void AddInt(int value)
            => AddBytes(_bitConverter.GetBytes(value));

        protected int GetInt()
            => _bitConverter.ToInt32(GetBytes(sizeof(int)), 0);

        protected void AddUUID(Guid uuid)
        {
            var data = uuid.ToByteArray();

            _bitConverter.ToUInt64(data, 8);
            _bitConverter.ToUInt64(data, 0);

            AddBytes(data);
        }

        protected Guid GetUUID()
        {
            var longB = GetULong();
            var longA = GetULong();
            var data = new byte[16];

            Array.Copy(_bitConverter.GetBytes(longA), data, 8);
            Array.Copy(_bitConverter.GetBytes(longB), 0, data, 8, 8);

            return new Guid(data);
        }
    }
}
