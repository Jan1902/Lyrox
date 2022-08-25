namespace Lyrox.Networking.Types
{
    internal static class VarInt
    {
        public const int MIN_SIZE = 1;
        public const int MAX_SIZE = 5;

        public static int ReadVarIntFromStream(Stream stream)
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = (byte)stream.ReadByte();
                int value = (read & 0x7f);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0x80) != 0);

            return result;
        }

        public static void WriteVarIntToStream(Stream stream, int value)
        {
            do
            {
                byte temp = (byte)(value & 127);
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
            using var stream = new MemoryStream();
            do
            {
                byte temp = (byte)(value & 127);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 128;
                }
                stream.WriteByte(temp);
            } while (value != 0);

            return stream.ToArray();
        }

        public static byte[] ToBytesAsVarInt(this int value)
            => VarIntToBytes(value);

        public static int ReadVarInt(this Stream stream)
            => ReadVarIntFromStream(stream);

        public static void WriteVarInt(this Stream stream, int value)
            => WriteVarIntToStream(stream, value);
    }
}
