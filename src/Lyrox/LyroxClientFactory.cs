using Autofac;
using Lyrox.Abstraction;
using Lyrox.Chat;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events;
using Lyrox.Core.Modules;
using Lyrox.Networking;
using Lyrox.WorldData;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Lyrox
{
    public static class LyroxClientFactory
    {
        public static ILyroxClient GetLyroxClient(LyroxConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            var loggerConfiguration = new LoggerConfiguration().WriteTo.Console();
            builder.RegisterSerilog(loggerConfiguration);

            builder.RegisterInstance(configuration);
            builder.RegisterType<LyroxClient>().As<ILyroxClient>();

            var eventManager = new EventManager(builder);
            builder.RegisterInstance(eventManager).As<IEventManager>();

            var moduleManager = new ModuleManager();
            moduleManager.RegisterModule<ChatModule>();
            moduleManager.RegisterModule<WorldDataModule>();
            moduleManager.RegisterModule<NetworkingModule>();

            moduleManager.LoadModules(builder);
            moduleManager.RegisterEventHandlers(eventManager);

            var container = builder.Build();
            eventManager.RegisterEventHandlersFromContainer(container);

            return container.Resolve<ILyroxClient>();
        }
    }
}
