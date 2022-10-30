using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ClientBound
{
    internal class DisconnectLogin : MojangClientBoundPacketBase
    {
        public string Message { get; private set; }

        public override void Parse()
            => Message = Reader.ReadStringWithVarIntPrefix();
    }
}
