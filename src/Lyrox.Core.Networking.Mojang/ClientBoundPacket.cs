namespace Lyrox.Core.Networking.Mojang
{
    public abstract class ClientBoundPacket : NetworkPacketBase
    {
        public abstract void Parse();
    }
}
