using Autofac;
using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Configuration;

namespace Lyrox.Framework;

public class ModuleManager : IModuleManager
{
    private readonly List<IModule> _modules;
    private readonly ILyroxConfiguration _lyroxConfiguration;

    public ModuleManager(ILyroxConfiguration lyroxConfiguration)
    {
        _modules = new();
        _lyroxConfiguration = lyroxConfiguration;
    }

    public void RegisterModule<T>() where T : IModule, new()
    {
        var instance = new T();
        _modules.Add(instance);
    }

    public PacketTypeMapping LoadModules(ContainerBuilder builder)
    {
        var packetMapping = new PacketTypeMapping();
        var serviceContainer = new ServiceContainer();

        foreach (var module in _modules)
            module.Load(serviceContainer, packetMapping, _lyroxConfiguration);

        foreach (var (As, Type) in serviceContainer.GetTypeServices())
            builder.RegisterType(Type).As(As).InstancePerLifetimeScope().AutoActivate();

        foreach(var (As, Instance) in serviceContainer.GetInstanceServices())
            builder.RegisterInstance(Instance).As(As);

        return packetMapping;
    }
}
