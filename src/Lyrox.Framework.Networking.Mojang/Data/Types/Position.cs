using BitConverter;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking.Mojang.Data.Types;

internal static class Position
{
    public static Vector3i ReadPositionFromStream(Stream stream)
    {
        var reader = new MojangBinaryReader(stream);
        var value = reader.ReadLong();

        var x = value >> 38;
        var y = value << 52 >> 52;
        var z = value << 26 >> 38;

        return new Vector3i((int)x, (int)y, (int)z);
    }

    public static void WritePositionToStream(Stream stream, Vector3i value)
    {
        var writer = new MojangBinaryWriter(stream);
        
        var result = ((value.X & 0x3FFFFFF) << 38) | ((value.Z & 0x3FFFFFF) << 12) | (value.Y & 0xFFF);
        writer.WriteLong(result);
    }

    public static Vector3i ReadPosition(this Stream stream)
        => ReadPositionFromStream(stream);

    public static void WritePosition(this Stream stream, Vector3i value)
        => WritePositionToStream(stream, value);
}
