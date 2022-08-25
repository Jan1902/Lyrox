using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrox.Networking.Packets.ClientBound
{
    internal abstract class ClientBoundPacket : NetworkPacketBase
    {
        public abstract void Parse();
    }
}
