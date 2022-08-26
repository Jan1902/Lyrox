using Lyrox.Core.Events;
using Lyrox.Networking.Events;

namespace Lyrox.Networking.Packets.ClientBound
{
    internal class KeepAlive : ClientBoundPacket, IMappedToEvent
    {
        public long KeepAliveID { get; private set; }

        public Event GetEvent()
            => new KeepAliveEvent(KeepAliveID);

        public override void Parse()
            => KeepAliveID = GetLong();
    }
}
