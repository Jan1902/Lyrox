using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Core.Abstraction.Modules;

public interface IModule
{
    void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration);
}
