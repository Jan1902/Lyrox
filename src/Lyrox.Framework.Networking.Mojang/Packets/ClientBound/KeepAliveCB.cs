using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ClientBound
{
    public class KeepAliveCB : MojangClientBoundPacket
    {
        public long KeepAliveID { get; private set; }

        public override void Parse()
            => KeepAliveID = Reader.ReadLong();
    }
}
