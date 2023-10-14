using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Networking.Core.Data.Abstraction;

public interface IMinecraftBinaryReaderWriterFactory
{
    IMinecraftBinaryReader CreateBinaryReader(Stream stream);
    IMinecraftBinaryWriter CreateBinaryWriter(Stream stream);
}
