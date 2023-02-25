using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Player.Mojang.Packets.ClientBound;

internal class SynchronizePlayerPosition : MojangClientBoundPacketBase
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }
    public float Yaw { get; private set; }
    public float Pitch { get; private set; }
    public byte Flags { get; private set; }
    public int TeleportID { get; private set; }
    public bool DismountVehicle { get; private set; }

    public override void Parse()
    {
        X = Reader.ReadDouble();
        Y = Reader.ReadDouble();
        Z = Reader.ReadDouble();
        Yaw = Reader.ReadFloat();
        Pitch = Reader.ReadFloat();
        Flags = Reader.ReadByte();
        TeleportID = Reader.ReadVarInt();
        DismountVehicle = Reader.ReadBool();
    }

    [Flags]
    public enum RelativeValues
    {
        X = 0x01,
        Y = 0x02,
        Z = 0x04,
        Y_ROT = 0x08,
        X_ROT = 0x10
    }
}
