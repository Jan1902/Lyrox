namespace Lyrox.Networking.Connection
{
    public interface INetworkConnection
    {
        Task Connect();
        Task SendPacket(int opCode, byte[] data);
    }
}
