using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Models.World
{
    public class BlockEntity
    {
        public int Type { get; set; }
        public Vector3i Position { get; set; }
        public NBTTag Data { get; set; }
    }
}
