using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Lyrox.Framework.WorldData.Mojang.Packets;

namespace Lyrox.Framework.WorldData.Mojang.PacketHandlers
{
    public class WorldDataPacketHandler : IPacketHandler<ChunkData>
    {
        public void HandlePacket(ChunkData networkPacket)
        {
            Console.WriteLine($"Received {networkPacket.Data.Length} bytes of Chunk Data for Chunk {networkPacket.ChunkX}/{networkPacket.ChunkZ}");
        }
    }
}
