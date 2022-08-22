using Autofac;
using Lyrox.Core.Events;

namespace Lyrox.Core.Modules
{
    public class ModuleManager : IModuleManager
    {
        private readonly List<IModule> _modules;

        public ModuleManager()
        {
            _modules = new();
        }

        public void RegisterModule<T>() where T : IModule, new()
        {
            _modules.Add(new T());
        }

        public void LoadModules(ContainerBuilder builder)
        {
            foreach (var module in _modules)
                module.Load(builder);
        }

        public void RegisterEventHandlers(IEventManager eventManager)
        {
            foreach (var module in _modules)
                module.RegisterEventHandlers(eventManager);
        }
    }
}
