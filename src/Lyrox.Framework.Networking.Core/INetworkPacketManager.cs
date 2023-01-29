namespace Lyrox.Framework.Core.Abstraction.Networking;

public interface INetworkPacketManager
{
    Task HandleNetworkPacket(int opCode, byte[] data);
}
