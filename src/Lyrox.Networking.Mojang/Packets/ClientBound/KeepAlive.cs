using Lyrox.Networking.Mojang.Packets.Base;

namespace Lyrox.Networking.Mojang.Packets.ClientBound
{
    public class KeepAliveCB : MojangClientBoundPacket
    {
        public long KeepAliveID { get; private set; }

        public override void Parse()
            => KeepAliveID = Reader.ReadLong();
    }
}
