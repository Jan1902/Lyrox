using Lyrox.Core.Events;

namespace Lyrox.Networking.Parsing
{
    public interface INetworkPacketParser
    {
        Event? ParsePacket(int opCode, byte[] data);
    }
}