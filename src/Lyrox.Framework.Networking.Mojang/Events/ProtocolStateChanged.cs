using Lyrox.Framework.Core.Events;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.Events
{
    internal record ProtocolStateChangedEvent : EventBase
    {
        public ProtocolState ProtocolState { get; init; }

        public ProtocolStateChangedEvent(ProtocolState protocolState)
            => ProtocolState = protocolState;
    }
}
