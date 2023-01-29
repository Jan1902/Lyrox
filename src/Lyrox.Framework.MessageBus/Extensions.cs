using Autofac;
using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Base.Messaging.Internal;
using Lyrox.Framework.Base.Messaging.Internal.Autofac;

namespace Lyrox.Framework.Base.Messaging;

public static class Extensions
{
    public static void RegisterMessageBus(this ContainerBuilder builder)
        => builder.RegisterType<MessageBus>()
            .As<IMessageBus>()
            .As<IPublishBus>()
            .As<ISubscribeBus>()
            .InstancePerLifetimeScope();

    public static void RegisterAutofacMessagebus(this ContainerBuilder builder)
        => builder.RegisterType<AutofacMessageBus>()
            .As<AutofacMessageBus>()
            .As<IMessageBus>()
            .As<IPublishBus>()
            .As<ISubscribeBus>()
            .InstancePerLifetimeScope();

    public static void SetupAutofacMessageBus(this IContainer container)
        => container.Resolve<AutofacMessageBus>().Setup();
}
