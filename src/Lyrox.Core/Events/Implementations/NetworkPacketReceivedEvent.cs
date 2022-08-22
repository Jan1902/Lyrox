using Lyrox.Core.Models.Networking;

namespace Lyrox.Core.Events.Implementations
{
    public class NetworkPacketReceivedEvent<T> : Event where T : NetworkPacket
    {
        public T Packet { get; set; }

        public NetworkPacketReceivedEvent(object sender, T packet)
        {
            Packet = packet;
        }
    }
}
