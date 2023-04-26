using Lyrox.Framework.Core.Models.Inventory;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Inventory.Mojang.Packets;

internal class SetContainerContent : MojangClientBoundPacketBase
{
    public byte WindowID { get; private set; }
    public int StateID { get; private set; }
    public Slot[] Slots { get; set; }
    public Slot CarriedItem { get; set; }

    public override void Parse()
    {
        WindowID = Reader.ReadByte();
        StateID = Reader.ReadVarInt();

        var length = Reader.ReadVarInt();
        Slots = new Slot[length];

        for (var i = 0; i < length; i++)
            Slots[i] = ParseSlot();

        CarriedItem = ParseSlot();
    }

    private Slot ParseSlot()
    {
        var slot = new Slot();
        slot.Present = Reader.ReadBool();

        if (slot.Present)
        {
            slot.ItemID = Reader.ReadVarInt();
            slot.ItemCount = Reader.ReadByte();
            slot.NBT = Reader.ReadNBTTag();
        }

        return slot;
    }
}
