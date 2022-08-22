using Autofac;
using Lyrox.Core.Abstraction;
using Lyrox.Core.Events;
using Lyrox.Core.Modules;

namespace Lyrox.WorldData
{
    public class WorldDataModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorldDataManager>().As<IWorldDataManager>();
        }

        public void RegisterEventHandlers(IEventManager eventManager)
        {

        }
    }
}
