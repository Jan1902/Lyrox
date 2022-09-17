namespace Lyrox.Core.Networking.Mojang
{
    public abstract class ServerBoundPacket : NetworkPacketBase
    {
        public abstract byte[] Build();
        public abstract int OPCode { get; }
    }
}
