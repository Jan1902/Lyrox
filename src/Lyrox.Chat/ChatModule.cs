using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Modules.Abstractions;
using Lyrox.Core.Networking.Abstraction;

namespace Lyrox.Chat
{
    public class ChatModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            serviceContainer.RegisterType<IChatManager, ChatManager>();
        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {

        }
    }
}
