using Lyrox.Core.Events;

namespace Lyrox.Networking.Events
{
    internal class KeepAliveEvent : Event
    {
        public long KeepAliveID { get; }

        public KeepAliveEvent(long keepAliveID)
            => KeepAliveID = keepAliveID;
    }
}
