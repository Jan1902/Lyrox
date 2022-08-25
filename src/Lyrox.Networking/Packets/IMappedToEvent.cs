using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Core.Events;

namespace Lyrox.Networking.Packets
{
    internal interface IMappedToEvent
    {
        Event GetEvent();
    }
}
