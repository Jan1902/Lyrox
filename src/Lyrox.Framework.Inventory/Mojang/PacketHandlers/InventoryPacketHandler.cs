using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Lyrox.Framework.Inventory.Mojang.Packets;

namespace Lyrox.Framework.Inventory.Mojang.PacketHandlers
{
    internal class InventoryPacketHandler : IPacketHandler<SetContainerContent>
    {
        private readonly IInventoryManager _inventoryManager;

        public InventoryPacketHandler(IInventoryManager inventoryManager)
            => _inventoryManager = inventoryManager;

        public Task HandlePacket(SetContainerContent networkPacket)
        {
            if (networkPacket.WindowID == 0)
                _inventoryManager.PlayerInventory = networkPacket.Slots;

            return Task.CompletedTask;
        }
    }
}
