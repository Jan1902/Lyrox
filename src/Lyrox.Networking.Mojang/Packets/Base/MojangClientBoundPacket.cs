using Lyrox.Core.Networking.Abstraction.Packet;
using Lyrox.Networking.Mojang.Data;

namespace Lyrox.Networking.Mojang.Packets.Base
{
    public abstract class MojangClientBoundPacket : IClientBoundNetworkPacket
    {
        protected IMojangBinaryReader Reader;

        public void ParsePacket(byte[] data)
        {
            Reader = new MojangBinaryReader(new MemoryStream(data));
            Parse();
        }

        public abstract void Parse();
    }
}
