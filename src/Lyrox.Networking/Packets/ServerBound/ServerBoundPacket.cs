namespace Lyrox.Networking.Packets.ServerBound
{
    public abstract class ServerBoundPacket : NetworkPacketBase
    {
        public abstract byte[] Build();
        public abstract int OPCode { get; }
    }
}
