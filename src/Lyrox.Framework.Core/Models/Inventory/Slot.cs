using Lyrox.Framework.Core.Models.NBT;

namespace Lyrox.Framework.Core.Models.Inventory
{
    public class Slot
    {
        public bool Present { get; set; }
        public int ItemID { get; set; }
        public int ItemCount { get; set; }
        public NBTTag NBT { get; set; }
    }
}
