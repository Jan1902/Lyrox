namespace Lyrox.Framework.Shared.Types;

public record Vector3i
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3i operator +(Vector3i a, Vector3i b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3i operator -(Vector3i a, Vector3i b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
}
