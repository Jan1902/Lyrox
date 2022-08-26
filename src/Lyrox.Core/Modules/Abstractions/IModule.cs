using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;

namespace Lyrox.Core.Modules.Abstractions
{
    public interface IModule
    {
        void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration);
        void RegisterEventHandlers(IEventManager eventManager, LyroxConfiguration lyroxConfiguration);
    }
}
