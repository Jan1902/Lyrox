//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lyrox.Networking.Packets
//{
//    public class NetworkPacketBase
//    {
//        private List<byte> _bytes = new();

//        public NetworkPacketBase(OPHandshaking op)
//        {
//            AddVarInt((byte)op);
//        }

//        public NetworkPacketBase(OPLogin op)
//        {
//            AddVarInt((byte)op);
//        }

//        public NetworkPacketBase(OPPlay op)
//        {
//            AddVarInt((byte)op);
//        }

//        public NetworkPacketBase(byte[] data)
//        {
//            _bytes = data.ToList();
//        }

//        public void AddVarInt(int value)
//        {
//            _bytes.AddRange(VarInt.VarIntToBytes(value));
//        }

//        public int GetVarInt()
//        {
//            var value = VarInt.ReadVarInt(new MemoryStream(_bytes.ToArray()));
//            _bytes.RemoveRange(0, VarInt.VarIntToBytes(value).Length);
//            return value;
//        }

//        public void AddBytes(byte[] value)
//        {
//            _bytes.AddRange(value);
//        }

//        public byte[] GetBytes(int count)
//        {
//            var value = _bytes.GetRange(0, count).ToArray();
//            _bytes.RemoveRange(0, count);
//            return value;
//        }

//        public void AddUShort(ushort value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public ushort GetUShort()
//        {
//            var value = EndianBitConverter.Big.ToUInt16(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(ushort));
//            return value;
//        }

//        public void AddShort(short value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public short GetShort()
//        {
//            var value = EndianBitConverter.Big.ToInt16(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(short));
//            return value;
//        }

//        public void AddString(string value)
//        {
//            AddVarInt(Encoding.UTF8.GetBytes(value).Length);
//            _bytes.AddRange(Encoding.UTF8.GetBytes(value));
//        }

//        public string GetString()
//        {
//            var length = GetVarInt();
//            var value = Encoding.UTF8.GetString(_bytes.GetRange(0, length).ToArray());
//            _bytes.RemoveRange(0, length);
//            return value;
//        }

//        public string GetString(int length)
//        {
//            var value = Encoding.UTF8.GetString(_bytes.GetRange(0, length).ToArray());
//            _bytes.RemoveRange(0, length);
//            return value;
//        }

//        public void AddByte(byte value)
//        {
//            _bytes.Add(value);
//        }

//        public byte GetByte()
//        {
//            var value = _bytes[0];
//            _bytes.RemoveRange(0, 1);
//            return value;
//        }

//        public bool GetBool()
//        {
//            var value = _bytes[0] == 1 ? true : false;
//            _bytes.RemoveRange(0, 1);
//            return value;
//        }

//        public void AddBool(bool value)
//        {
//            _bytes.Add(value ? (byte)1 : (byte)0);
//        }

//        public void AddDouble(double value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public double GetDouble()
//        {
//            var value = EndianBitConverter.Big.ToDouble(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(double));
//            return value;
//        }

//        public float GetFloat()
//        {
//            var value = EndianBitConverter.Big.ToSingle(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(float));
//            return value;
//        }

//        public void AddFloat(float value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public void AddLong(long value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public long GetLong()
//        {
//            var value = EndianBitConverter.Big.ToInt64(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(long));
//            return value;
//        }

//        public ulong GetULong()
//        {
//            var value = EndianBitConverter.Big.ToUInt64(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(ulong));
//            return value;
//        }

//        public int GetInt()
//        {
//            var value = EndianBitConverter.Big.ToInt32(_bytes.ToArray(), 0);
//            _bytes.RemoveRange(0, sizeof(int));
//            return value;
//        }

//        public void AddInt(int value)
//        {
//            _bytes.AddRange(EndianBitConverter.Big.GetBytes(value));
//        }

//        public byte[] GetLengthLessBytes()
//        {
//            return _bytes.ToArray();
//        }
//    }
//}
