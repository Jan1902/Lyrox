namespace Lyrox.Networking.Packets
{
    internal interface IPacketHandler
    {
        void HandlePacket(int opCode, byte[] data);
    }
}