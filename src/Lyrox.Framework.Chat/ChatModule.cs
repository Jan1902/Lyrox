using Lyrox.Framework.Chat.Mojang;
using Lyrox.Framework.Chat.Mojang.PacketHandlers;
using Lyrox.Framework.Chat.Mojang.Packets.ClientBound;
using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Exceptions;
using Lyrox.Framework.Core.Modules.Abstractions;
using Lyrox.Framework.Core.Networking.Abstraction;
using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Chat
{
    public class ChatModule : IModule
    {
        public void Load(IServiceContainer serviceContainer, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
            {
                serviceContainer.RegisterType<IChatManager, ChatManager>();
                serviceContainer.RegisterType<IJSONChatParser, JSONChatParser>();
            }
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }

        public void RegisterPacketHandlers(INetworkPacketManager networkPacketManager, LyroxConfiguration lyroxConfiguration)
        {
            if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
                networkPacketManager.RegisterNetworkPacketHandler<PlayerChatMessage, ChatPacketHandler>(0x33);
            else
                throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
        }
    }
}
