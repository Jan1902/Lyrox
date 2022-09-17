using Lyrox.Core.Events;
using Lyrox.Networking.Mojang.Types;

namespace Lyrox.Networking.Mojang.Events
{
    internal class ProtocolStateChanged : Event
    {
        public ProtocolState ProtocolState { get; set; }

        public ProtocolStateChanged(ProtocolState protocolState)
            => ProtocolState = protocolState;
    }
}
