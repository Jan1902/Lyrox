using Lyrox.Networking.Mojang;
using Lyrox.Networking.Mojang.Packets.Base;

namespace Lyrox.WorldData.Mojang.Packets
{
    public class ChunkData : MojangClientBoundPacket
    {
        public int ChunkX { get; private set; }
        public int ChunkZ { get; private set; }
        public byte[] Data { get; private set; }

        public override void Parse()
        {
            ChunkX = Reader.ReadInt();
            ChunkZ = Reader.ReadInt();

            // NBT Data is irrelevant for now
            _ = MojangNBTReader.ParseNBT(Reader);

            Data = Reader.ReadBytes(Reader.ReadVarInt());

            // Block Entites are irrelevant for now
            // Light Data is irrelevant
        }
    }
}
