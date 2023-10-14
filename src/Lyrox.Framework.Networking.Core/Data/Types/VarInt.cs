namespace Lyrox.Framework.Networking.Mojang.Data.Types;

public static class VarInt
{
    public const int MIN_SIZE = 1;
    public const int MAX_SIZE = 5;

    public static byte[] VarIntToBytes(int value)
    {
        using var stream = new MemoryStream();
        do
        {
            var temp = (byte)(value & 127);
            value >>= 7;
            if (value != 0)
                temp |= 128;
            stream.WriteByte(temp);
        } while (value != 0);

        return stream.ToArray();
    }

    public static byte[] ToBytesAsVarInt(this int value)
        => VarIntToBytes(value);
}
