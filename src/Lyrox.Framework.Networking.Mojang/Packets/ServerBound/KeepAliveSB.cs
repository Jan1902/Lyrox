using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ServerBound
{
    internal class KeepAliveSB : MojangServerBoundPacket
    {
        public long KeepAliveID { get; }

        public override int OPCode => 0x11;

        public KeepAliveSB(long keepAliveID)
            => KeepAliveID = keepAliveID;

        public override void Build()
            => Writer.WriteLong(KeepAliveID);
    }
}
