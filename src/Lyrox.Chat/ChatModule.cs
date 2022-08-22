using Autofac;
using Lyrox.Core.Abstraction;
using Lyrox.Core.Events;
using Lyrox.Core.Modules;

namespace Lyrox.Chat
{
    public class ChatModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChatManager>().As<IChatManager>();
        }

        public void RegisterEventHandlers(IEventManager eventManager)
        {
            //eventManager.RegisterEventHandler<NetworkPacketReceivedEvent<ChatPacket>, PacketEventHandler>();
        }
    }
}
