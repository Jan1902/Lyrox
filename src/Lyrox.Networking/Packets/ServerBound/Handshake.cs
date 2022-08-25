using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Networking.Types;

namespace Lyrox.Networking.Packets.ServerBound
{
    internal class Handshake : ServerBoundPacket
    {
        public int ProtocolVersion { get; }
        public string ServerAdress { get; }
        public ushort ServerPort { get; }
        public int NextState { get; }

        public Handshake(int protocolVersion, string serverAdress, ushort serverPort, int nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAdress = serverAdress;
            ServerPort = serverPort;
            NextState = nextState;
        }

        public override byte[] Build()
        {
            AddVarInt((int)OPHandshaking.Handshake);

            AddVarInt(ProtocolVersion);
            AddString(ServerAdress);
            AddUShort(ServerPort);
            AddVarInt(NextState);

            return GetBytes();
        }
    }
}
