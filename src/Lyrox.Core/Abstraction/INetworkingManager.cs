namespace Lyrox.Core.Abstraction
{
    public interface INetworkingManager
    {
        Task Connect();
        Task SendStartPackets();
    }
}
