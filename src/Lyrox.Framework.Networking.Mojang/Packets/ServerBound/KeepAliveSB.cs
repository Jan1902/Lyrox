using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ServerBound;

internal class KeepAliveSB : MojangServerBoundPacketBase
{
    public long KeepAliveID { get; init; }

    public override int OPCode => 0x12;

    public KeepAliveSB(long keepAliveID)
        => KeepAliveID = keepAliveID;

    public override void Build()
        => Writer.WriteLong(KeepAliveID);
}
