using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Models.Inventory;

namespace Lyrox.Framework.Inventory.Mojang
{
    public class InventoryManager : IInventoryManager
    {
        public Slot[] PlayerInventory { get; set; }
    }
}
