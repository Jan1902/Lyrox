namespace Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler
{
    public interface IPacketHandler<T> where T : IClientBoundNetworkPacket
    {
        void HandlePacket(T networkPacket);
    }
}
