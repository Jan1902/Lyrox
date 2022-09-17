using Lyrox.Core.Networking.Abstraction.Packet;
using Lyrox.Networking.Mojang.Data;

namespace Lyrox.Networking.Mojang.Packets.Base
{
    public abstract class MojangServerBoundPacket : IServerBoundNetworkPacket
    {
        protected IMojangBinaryWriter Writer;

        public byte[] BuildPacket()
        {
            var stream = new MemoryStream();
            Writer = new MojangBinaryWriter(stream);
            Build();
            var pos = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            var data = new byte[pos];
            stream.Read(data, 0, data.Length);
            return data;
        }

        public abstract void Build();

        public abstract int OPCode { get; }
    }
}
