using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Mojang.Packets.Base;

public abstract class MojangClientBoundPacketBase : IClientBoundNetworkPacket
{
    protected IMojangBinaryReader Reader;

    public void ParsePacket(byte[] data)
    {
        Reader = new MojangBinaryReader(new MemoryStream(data));
        Parse();
    }

    public abstract void Parse();
}
