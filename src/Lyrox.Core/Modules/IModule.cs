using Autofac;
using Lyrox.Core.Events;

namespace Lyrox.Core.Modules
{
    public interface IModule
    {
        void Load(ContainerBuilder builder);
        void RegisterEventHandlers(IEventManager eventManager);
    }
}
