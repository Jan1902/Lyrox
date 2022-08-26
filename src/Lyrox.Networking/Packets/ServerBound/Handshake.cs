namespace Lyrox.Networking.Packets.ServerBound
{
    internal class Handshake : ServerBoundPacket
    {
        public int ProtocolVersion { get; }
        public string ServerAddress { get; }
        public ushort ServerPort { get; }
        public int NextState { get; }

        public override int OPCode => 0x00;

        public Handshake(int protocolVersion, string serverAdress, ushort serverPort, int nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAddress = serverAdress;
            ServerPort = serverPort;
            NextState = nextState;
        }

        public override byte[] Build()
        {
            AddVarInt(ProtocolVersion);
            AddString(ServerAddress);
            AddUShort(ServerPort);
            AddVarInt(NextState);

            return GetBytes();
        }
    }
}
