using Lyrox.Framework.Chat.Mojang.Packets.ClientBound;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Microsoft.Extensions.Logging;

namespace Lyrox.Framework.Chat.Mojang.PacketHandlers
{
    internal class ChatPacketHandler : IPacketHandler<PlayerChatMessage>
    {
        private readonly IJSONChatParser _jsonChatParser;
        private readonly ILogger<ChatPacketHandler> _logger;
        private readonly IEventManager _eventManager;
        private readonly LyroxConfiguration _lyroxConfiguration;

        public ChatPacketHandler(IJSONChatParser jsonChatParser, ILogger<ChatPacketHandler> logger, IEventManager eventManager, LyroxConfiguration lyroxConfiguration)
        {
            _jsonChatParser = jsonChatParser;
            _logger = logger;
            _eventManager = eventManager;
            _lyroxConfiguration = lyroxConfiguration;
        }

        public async Task HandlePacket(PlayerChatMessage networkPacket)
        {
            var sender = _jsonChatParser.ParseChatJson(networkPacket.NetworkName);

            if (sender == _lyroxConfiguration.Username)
                return;

            _logger.LogInformation("[Chat] {sender}: {text}", sender, networkPacket.PlainMessage);
            await _eventManager.PublishEvent(new ChatMessageReceivedEvent(networkPacket.PlainMessage, sender));
        }
    }
}
