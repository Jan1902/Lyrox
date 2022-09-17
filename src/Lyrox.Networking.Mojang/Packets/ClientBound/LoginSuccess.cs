using Lyrox.Networking.Mojang.Packets.Base;

namespace Lyrox.Networking.Mojang.Packets.ClientBound
{
    public class LoginSuccess : MojangClientBoundPacket
    {
        public Guid UUID { get; private set; }
        public string Username { get; private set; }

        public override void Parse()
        {
            UUID = Reader.ReadUUID();
            Username = Reader.ReadStringWithVarIntPrefix();
        }
    }
}
