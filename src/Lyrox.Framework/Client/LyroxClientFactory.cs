using Autofac;
using Lyrox.Framework.Base.Messaging;
using Lyrox.Framework.Chat;
using Lyrox.Framework.Client.Abstraction;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Inventory;
using Lyrox.Framework.Networking;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Core.Data.Abstraction;
using Lyrox.Framework.Player;
using Lyrox.Framework.World;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Lyrox.Framework.Client;

public static class LyroxClientFactory
{
    private const string LogFilePath = "Log.txt";

    public static ILyroxClient GetLyroxClient(ILyroxConfiguration configuration, Action<IModuleManager>? pluginBuilding = null)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
            .WriteTo.File(LogFilePath);

        var builder = new ContainerBuilder();
        builder.RegisterSerilog(loggerConfiguration);

        builder.RegisterInstance(configuration);
        builder.RegisterType<LyroxClient>().As<ILyroxClient>().InstancePerLifetimeScope();

        builder.RegisterAutofacMessagebus();

        var moduleManager = new ModuleManager(configuration);
        RegisterModules(moduleManager);
        pluginBuilding?.Invoke(moduleManager);
        var packetMapping = moduleManager.LoadModules(builder);

        builder.RegisterInstance(packetMapping);
        builder.RegisterType<NetworkPacketManager>().As<INetworkPacketManager>();
        builder.RegisterType<PacketSerializer>().As<IPacketSerializer>();

        var container = builder.Build();
        container.SetupAutofacMessageBus();

        return container.Resolve<ILyroxClient>();
    }

    private static void RegisterModules(IModuleManager moduleManager)
    {
        moduleManager.RegisterModule<ChatModule>();
        moduleManager.RegisterModule<PlayerModule>();
        moduleManager.RegisterModule<WorldDataModule>();
        moduleManager.RegisterModule<NetworkingModule>();
        moduleManager.RegisterModule<InventoryModule>();
    }
}
