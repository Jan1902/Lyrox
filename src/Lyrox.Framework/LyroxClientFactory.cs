using Autofac;
using Lyrox.Framework.Abstraction;
using Lyrox.Framework.Chat;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Modules;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Networking;
using Lyrox.Framework.Player;
using Lyrox.Framework.WorldData;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Lyrox.Framework
{
    public static class LyroxClientFactory
    {
        private const string LogFilePath = "Log.txt";

<<<<<<< HEAD
        public static ILyroxClient GetLyroxClient(LyroxConfiguration configuration, Action<IModuleManager>? pluginBuilding = null)
=======
        public static ILyroxClient GetLyroxClient(LyroxConfiguration configuration)
>>>>>>> db4aa368f623507e9e82bee13b108847e82d382d
        {
            var builder = new ContainerBuilder();
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(LogFilePath);

            builder.RegisterSerilog(loggerConfiguration);

            builder.RegisterInstance(configuration);
            builder.RegisterType<LyroxClient>().As<ILyroxClient>().InstancePerLifetimeScope();

            var eventManager = new EventManager(builder);
            builder.RegisterInstance(eventManager).As<IEventManager>();

            var packetManager = new NetworkPacketManager(builder);
            builder.RegisterInstance(packetManager).As<INetworkPacketManager>();

            var moduleManager = new ModuleManager(configuration);
            RegisterModules(moduleManager);
            pluginBuilding?.Invoke(moduleManager);

            moduleManager.LoadModuleServices(builder);
            moduleManager.RegisterEventHandlers(eventManager);
            moduleManager.RegisterPacketHandlers(packetManager);

            var container = builder.Build();
            eventManager.RegisterEventHandlersFromContainer(container);
            packetManager.RegisterPacketHandlersFromContainer(container);

            return container.Resolve<ILyroxClient>();
        }

        private static void RegisterModules(ModuleManager moduleManager)
        {
            moduleManager.RegisterModule<ChatModule>();
            moduleManager.RegisterModule<PlayerModule>();
            moduleManager.RegisterModule<WorldDataModule>();
            moduleManager.RegisterModule<NetworkingModule>();
        }
    }
}
