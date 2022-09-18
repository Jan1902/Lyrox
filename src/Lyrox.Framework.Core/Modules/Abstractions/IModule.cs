using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Networking.Abstraction;

namespace Lyrox.Framework.Core.Modules.Abstractions
{
    public interface IModule
    {
        void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration) { }
        void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration) { }
        void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration) { }
    }
}
