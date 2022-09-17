using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Networking.Abstraction;

namespace Lyrox.Core.Modules.Abstractions
{
    public interface IModule
    {
        void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration) { }
        void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration) { }
        void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration) { }
    }
}
