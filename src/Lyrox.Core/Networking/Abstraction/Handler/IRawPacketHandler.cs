namespace Lyrox.Core.Networking.Abstraction.Handler
{
    public interface IRawPacketHandler
    {
        public void HandlePacket(int opCode, byte[] data);
    }
}
