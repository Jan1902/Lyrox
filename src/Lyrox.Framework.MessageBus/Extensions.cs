using Autofac;
using Lyrox.Framework.Messaging.Abstraction;
using Lyrox.Framework.Messaging.Abstraction.Core;
using Lyrox.Framework.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Messaging.Abstraction.Messages;
using Lyrox.Framework.Messaging.Internal;
using Lyrox.Framework.Messaging.Internal.Autofac;

namespace Lyrox.Framework.Messaging
{
    public static class Extensions
    {
        public static void RegisterMessageBus(this ContainerBuilder builder)
            => builder.RegisterType<MessageBus>().As<IMessageBus>().InstancePerLifetimeScope();

        public static void RegisterAutofacMessagebus(this ContainerBuilder builder)
            => builder.RegisterType<AutoFacMessageBus>().As<IMessageBus>().InstancePerLifetimeScope();

        public static void RegisterMessageHandler<TMessage, THandler>(this ContainerBuilder builder)
            where TMessage : IMessage
            where THandler : IMessageHandler<TMessage>
            => builder.RegisterType<THandler>().As<IMessageHandler<TMessage>>().InstancePerDependency();

        public static void RegisterRequestHandler<TRequest, TResponse, THandler>(this ContainerBuilder builder)
            where TRequest : IRequest<TResponse>
            where THandler : IRequestHandler<TRequest, TResponse>
            where TResponse : IResponse
            => builder.RegisterType<THandler>().As<IRequestHandler<TRequest, TResponse>>().InstancePerDependency();
    }
}
