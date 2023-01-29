namespace Lyrox.Framework.Core.Abstraction.Networking.Packet;

public interface IServerBoundNetworkPacket
{
    int OPCode { get; }
    byte[] BuildPacket();
}
