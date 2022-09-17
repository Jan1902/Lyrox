using Lyrox.Networking.Mojang.Packets.Base;

namespace Lyrox.Networking.Mojang.Packets.ClientBound
{
    internal class DisconnectLogin : MojangClientBoundPacket
    {
        public string Message { get; private set; }

        public override void Parse()
            => Message = Reader.ReadStringWithVarIntPrefix();
    }
}
