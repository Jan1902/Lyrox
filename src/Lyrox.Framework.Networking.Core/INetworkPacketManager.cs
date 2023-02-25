namespace Lyrox.Framework.Networking.Core;

public interface INetworkPacketManager
{
    Task HandleNetworkPacket(int opCode, byte[] data);
}
