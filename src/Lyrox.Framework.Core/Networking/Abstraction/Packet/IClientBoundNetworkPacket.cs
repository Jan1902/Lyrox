namespace Lyrox.Framework.Core.Networking.Abstraction.Packet
{
    public interface IClientBoundNetworkPacket
    {
        void ParsePacket(byte[] data);
    }
}
