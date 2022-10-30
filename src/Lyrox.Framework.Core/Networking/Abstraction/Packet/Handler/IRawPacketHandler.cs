namespace Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler
{
    public interface IRawPacketHandler
    {
        Task HandleRawPacket(int opCode, byte[] data);
    }
}
