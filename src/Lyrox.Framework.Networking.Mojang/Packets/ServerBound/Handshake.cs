using Lyrox.Framework.Networking.Mojang.Packets.Base;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.Packets.ServerBound
{
    internal class Handshake : MojangServerBoundPacket
    {
        public int ProtocolVersion { get; }
        public string ServerAddress { get; }
        public ushort ServerPort { get; }
        public ProtocolState NextState { get; }

        public override int OPCode => 0x00;

        public Handshake(int protocolVersion, string serverAdress, ushort serverPort, ProtocolState nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAddress = serverAdress;
            ServerPort = serverPort;
            NextState = nextState;
        }

        public override void Build()
        {
            Writer.WriteVarInt(ProtocolVersion);
            Writer.WriteStringWithVarIntPrefix(ServerAddress);
            Writer.WriteUShort(ServerPort);
            Writer.WriteVarInt((int)NextState);
        }
    }
}
