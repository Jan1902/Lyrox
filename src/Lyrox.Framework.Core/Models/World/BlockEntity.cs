using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Models.World
{
    public class BlockEntity
    {
        public int Type { get; set; }
        public Vector3i Position { get; set; }
        public CompoundTag Data { get; set; }
    }
}
