using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Player.Mojang.Packets.ServerBound;

internal class ConfirmTeleportation : MojangServerBoundPacketBase
{
    public int TeleportID { get; init; }

    public override int OPCode => 0x00;

    public ConfirmTeleportation(int teleportID)
        => TeleportID = teleportID;

    public override void Build()
        => Writer.WriteVarInt(TeleportID);
}
