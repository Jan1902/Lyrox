namespace Lyrox.Networking.Packets.ClientBound
{
    internal abstract class ClientBoundPacket : NetworkPacketBase
    {
        public abstract void Parse();
    }
}
