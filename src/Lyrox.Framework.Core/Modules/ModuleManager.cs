using Autofac;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;

namespace Lyrox.Framework.Core.Modules
{
    public class ModuleManager : IModuleManager
    {
        private readonly List<IModule> _modules;
        private readonly IServiceContainer _serviceContainer;
        private readonly LyroxConfiguration _lyroxConfiguration;

        public ModuleManager(LyroxConfiguration lyroxConfiguration)
        {
            _modules = new();
            _serviceContainer = new ServiceContainer();
            _lyroxConfiguration = lyroxConfiguration;
        }

        public void RegisterModule<T>() where T : IModule, new()
        {
            var instance = new T();
            _modules.Add(instance);
            instance.Load(_serviceContainer, _lyroxConfiguration);
        }

        public void LoadModuleServices(ContainerBuilder builder)
        {
            var typeServices = _serviceContainer.GetTypeServices();
            foreach (var typeService in typeServices)
                builder.RegisterType(typeService.Value).As(typeService.Key).InstancePerLifetimeScope();
        }

        public void RegisterEventHandlers(IEventManager eventManager)
        {
            foreach (var module in _modules)
                module.RegisterEventHandlers(eventManager, _lyroxConfiguration);
        }

        public void RegisterPacketHandlers(INetworkPacketManager packetManager)
        {
            foreach (var module in _modules)
                module.RegisterPacketHandlers(packetManager, _lyroxConfiguration);
        }
    }
}
