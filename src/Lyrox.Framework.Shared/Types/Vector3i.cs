namespace Lyrox.Framework.Shared.Types
{
    public class Vector3i
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

        public static bool operator ==(Vector3i a, Vector3i b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3i a, Vector3i b)
            => !(a == b);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is not Vector3i)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return this == (Vector3i)obj;
        }

        public override int GetHashCode()
            => HashCode.Combine(X, Y, Z);
    }
}
