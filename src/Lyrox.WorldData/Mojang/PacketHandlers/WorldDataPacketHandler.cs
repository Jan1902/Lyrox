using Lyrox.Core.Networking.Abstraction.Handler;
using Lyrox.WorldData.Mojang.Packets;

namespace Lyrox.WorldData.Mojang.PacketHandlers
{
    public class WorldDataPacketHandler : IPacketHandler<ChunkData>
    {
        public void HandlePacket(ChunkData networkPacket)
        {
            Console.WriteLine($"Received {networkPacket.Data.Length} bytes of Chunk Data for Chunk {networkPacket.ChunkX}/{networkPacket.ChunkZ}");
        }
    }
}
