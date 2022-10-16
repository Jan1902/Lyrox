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
        {
            if (a is null || b is null)
                return a is null && b is null;

            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }
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
            => HashCode.Combine(X, Y, Z);

        public double Magnitude
            => (double)Math.Sqrt(X * X + Y * Y + Z * Z);

        public static double CrossProduct(Vector3d a, Vector3d b)
            => a.Magnitude * b.Magnitude;

        public static Rotation Angle(Vector3d a, Vector3d b)
        {
            var delta = b - a;
            var yaw = -(float)(Math.Atan2(delta.X, delta.Z) / Math.PI * 180);
            if (yaw < 0)
                yaw = 360 + yaw;
            var pitch = (float)(-Math.Asin(delta.Y / delta.Magnitude) / Math.PI * 180);

            return new Rotation(pitch, yaw);
        }
    }
}
