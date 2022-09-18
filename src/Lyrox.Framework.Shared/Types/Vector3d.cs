namespace Lyrox.Framework.Shared.Types
{
    public class Vector3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
            => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3d operator -(Vector3d a, Vector3d b)
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static bool operator ==(Vector3d a, Vector3d b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3d a, Vector3d b)
            => !(a == b);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is not Vector3d)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return this == (Vector3d)obj;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
