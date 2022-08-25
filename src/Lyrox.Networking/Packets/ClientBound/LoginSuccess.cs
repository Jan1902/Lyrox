using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Core.Events;
using Lyrox.Networking.Events;

namespace Lyrox.Networking.Packets.ClientBound
{
    internal class LoginSuccess : ClientBoundPacket, IMappedToEvent
    {
        public Guid UUID { get; private set; }
        public string Username { get; private set; }

        public Event GetEvent()
            => new LoginSucessfulEvent(UUID, Username);

        public override void Parse()
        {
            UUID = GetUUID();
            Username = GetString();
        }
    }
}
