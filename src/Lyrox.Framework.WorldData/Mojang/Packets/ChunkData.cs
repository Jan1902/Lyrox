using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.Packets.Base;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.WorldData.Mojang.Packets
{
    public class ChunkData : MojangClientBoundPacketBase
    {
        public int ChunkX { get; private set; }
        public int ChunkZ { get; private set; }
        public byte[] Data { get; private set; }
        public BlockEntity[] BlockEntities { get; private set; }

        public override void Parse()
        {
            ChunkX = Reader.ReadInt();
            ChunkZ = Reader.ReadInt();

            // NBT Data is irrelevant for now
            var test = MojangNBTReader.ParseNBT(Reader);

            Data = Reader.ReadBytes(Reader.ReadVarInt());

            var length = Reader.ReadVarInt();
            BlockEntities = new BlockEntity[length];
            for (var i = 0; i < length; i++)
            {
                var packedXZ = Reader.ReadByte();
                var x = 0;
                var z = 0;

                BlockEntities[i] = new BlockEntity()
                {
                    Position = new Vector3i(x, Reader.ReadShort(), z),
                    Type = Reader.ReadVarInt(),
                    Data = MojangNBTReader.ParseNBT(Reader)
                };
            }

            // Light Data is irrelevant
        }
    }
}
