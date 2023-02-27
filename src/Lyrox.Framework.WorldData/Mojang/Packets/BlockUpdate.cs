using Lyrox.Framework.Networking.Mojang.Packets.Base;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.World.Mojang.Packets;

public class BlockUpdate : MojangClientBoundPacketBase
{
    public Vector3i Location { get; private set; }
    public int ID { get; private set; }

    public override void Parse()
    {
        Location = Reader.ReadPosition();
        ID = Reader.ReadVarInt();
    }
}
