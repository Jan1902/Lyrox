using Lyrox.Networking.Types;

namespace Lyrox.Networking.Packets
{
    internal interface IPacketHandler
    {
        void HandlePacket(int opCode, byte[] data);
        void SetProtocolState(ProtocolState protocolState);
    }
}
