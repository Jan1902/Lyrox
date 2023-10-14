using Lyrox.Framework.Networking.Core.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Mojang.Data;

public class MojangBinaryReaderWriterFactory : IMinecraftBinaryReaderWriterFactory
{
    public IMinecraftBinaryReader CreateBinaryReader(Stream stream)
        => new MojangBinaryReader(stream);

    public IMinecraftBinaryWriter CreateBinaryWriter(Stream stream)
        => new MojangBinaryWriter(stream);
}
