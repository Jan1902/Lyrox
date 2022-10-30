namespace Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler
{
    public interface IPacketHandler<T> where T : IClientBoundNetworkPacket
    {
        Task HandlePacket(T networkPacket);
    }
}
