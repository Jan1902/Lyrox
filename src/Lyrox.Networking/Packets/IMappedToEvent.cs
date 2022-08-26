using Lyrox.Core.Events;

namespace Lyrox.Networking.Packets
{
    internal interface IMappedToEvent
    {
        Event GetEvent();
    }
}
