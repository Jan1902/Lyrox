using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Chat.Mojang.Packets.ClientBound;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;
using Lyrox.Framework.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace Lyrox.Framework.Chat.Mojang.PacketHandlers;

internal class ChatPacketHandler : IPacketHandler<PlayerChatMessage>
{
    private readonly IJSONChatParser _jsonChatParser;
    private readonly ILogger<ChatPacketHandler> _logger;
    private readonly IMessageBus _messageBus;
    private readonly ILyroxConfiguration _lyroxConfiguration;

    public ChatPacketHandler(IJSONChatParser jsonChatParser, ILogger<ChatPacketHandler> logger, IMessageBus messageBus, ILyroxConfiguration lyroxConfiguration)
    {
        _jsonChatParser = jsonChatParser;
        _logger = logger;
        _messageBus = messageBus;
        _lyroxConfiguration = lyroxConfiguration;
    }

    public async Task HandlePacketAsync(PlayerChatMessage networkPacket)
    {
        var sender = _jsonChatParser.ParseChatJson(networkPacket.NetworkName);

        if (sender == _lyroxConfiguration.Username)
            return;

        _logger.LogInformation("[Chat] {sender}: {text}", sender, networkPacket.PlainMessage);

        await _messageBus.PublishAsync(new ChatMessageReceivedMessage(networkPacket.PlainMessage, sender));
    }
}
