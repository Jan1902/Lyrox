using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Configuration;

namespace Lyrox.Framework.Core.Abstraction.Modules;

public interface IModule
{
    void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration);
}
