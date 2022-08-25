using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrox.Networking.Packets.ServerBound
{
    internal abstract class ServerBoundPacket : NetworkPacketBase
    {
        public abstract byte[] Build();
    }
}
