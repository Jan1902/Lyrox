using Lyrox.Framework.Core.Models.Inventory;

namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface IInventoryManager
{
    public Slot[] PlayerInventory { get; set; }
}
