namespace Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler
{
    public interface IRawPacketHandler
    {
        public void HandlePacket(int opCode, byte[] data);
    }
}
