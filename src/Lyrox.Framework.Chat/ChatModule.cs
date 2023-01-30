using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Chat.Mojang;
using Lyrox.Framework.Chat.Mojang.PacketHandlers;
using Lyrox.Framework.Chat.Mojang.Packets.ClientBound;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Shared.Exceptions;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Chat;

public class ChatModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
    {
        if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
        {
            serviceContainer.RegisterType<IChatManager, ChatManager>();
            serviceContainer.RegisterType<IJSONChatParser, JSONChatParser>();

            serviceContainer.RegisterPacketHandler<PlayerChatMessage, ChatPacketHandler>(packetMapping, 0x33);
        }
        else
            throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
    }
}
