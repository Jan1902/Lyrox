using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Player.Mojang.Packets.ServerBound;

internal class SetPlayerPositionAndRotation : MojangServerBoundPacketBase
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public float Yaw { get; init; }
    public float Pitch { get; init; }
    public bool OnGround { get; init; }

    public override int OPCode => 0x15;

    public SetPlayerPositionAndRotation(double x, double y, double z, float yaw, float pitch, bool onGround)
    {
        X = x;
        Y = y;
        Z = z;
        Yaw = yaw;
        Pitch = pitch;
        OnGround = onGround;
    }

    public SetPlayerPositionAndRotation(Vector3d position, Rotation rotation, bool onGround)
    {
        X = position.X;
        Y = position.Y;
        Z = position.Z;
        Yaw = rotation.Yaw;
        Pitch = rotation.Pitch;
        OnGround = onGround;
    }

    public override void Build()
    {
        Writer.WriteDouble(X);
        Writer.WriteDouble(Y);
        Writer.WriteDouble(Z);
        Writer.WriteFloat(Yaw);
        Writer.WriteFloat(Pitch);
        Writer.WriteBool(OnGround);
    }
}
