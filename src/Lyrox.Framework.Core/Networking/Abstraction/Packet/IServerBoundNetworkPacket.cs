namespace Lyrox.Framework.Core.Networking.Abstraction.Packet
{
    public interface IServerBoundNetworkPacket
    {
        int OPCode { get; }
        byte[] BuildPacket();
    }
}
