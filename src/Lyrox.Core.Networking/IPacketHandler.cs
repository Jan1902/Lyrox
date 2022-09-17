using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Core.Networking.Mojang;

namespace Lyrox.Core.Networking
{
    public interface IPacketHandler<T> where T : ClientBoundPacket
    {

    }
}
