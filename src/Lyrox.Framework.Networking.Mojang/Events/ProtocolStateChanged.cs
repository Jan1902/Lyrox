using Lyrox.Framework.Core.Events;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.Events
{
    internal class ProtocolStateChanged : Event
    {
        public ProtocolState ProtocolState { get; set; }

        public ProtocolStateChanged(ProtocolState protocolState)
            => ProtocolState = protocolState;
    }
}
