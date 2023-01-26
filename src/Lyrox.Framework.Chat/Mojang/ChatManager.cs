﻿using Lyrox.Framework.Chat.Mojang.Packets.ServerBound;
using Lyrox.Framework.Core.Abstraction.Managers;

namespace Lyrox.Framework.Chat.Mojang;

public class ChatManager : IChatManager
{
    private readonly INetworkingManager _networkingManager;

    public ChatManager(INetworkingManager networkingManager)
        => _networkingManager = networkingManager;

    public Task SendChatMessage(string message)
        => _networkingManager.SendPacket(new ChatMessage(message));

    public Task SendCommand(string command, string[] arguments)
        => _networkingManager.SendPacket(new ChatCommand(command, arguments));

    public Task SendPrivateMessage(string message, string player)
        => _networkingManager.SendPacket(new ChatCommand("msg", player, message));
}
