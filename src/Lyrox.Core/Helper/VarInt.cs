namespace Lyrox.Core.Helper
{
    internal static class VarInt
    {
        public static int ReadVarInt(Stream stream)
        {
            var numRead = 0;
            var result = 0;
            byte read;
            do
            {
                read = (byte)stream.ReadByte();
                var value = read & 0x7f;
                result |= value << 7 * numRead;

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0x80) != 0);
            return result;
        }

        public static void WriteVarInt(int value, Stream stream)
        {
            do
            {
                var temp = (byte)(value & 127);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 128;
                }
                stream.WriteByte(temp);
            } while (value != 0);
        }

        public static byte[] VarIntToBytes(int value)
        {
            using (var stream = new MemoryStream())
            {
                do
                {
                    var temp = (byte)(value & 127);
                    value >>= 7;
                    if (value != 0)
                    {
                        temp |= 128;
                    }
                    stream.WriteByte(temp);
                } while (value != 0);
                return stream.ToArray();
            }
        }
    }
}
