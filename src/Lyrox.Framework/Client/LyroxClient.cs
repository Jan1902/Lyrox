using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Client.Abstraction;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Events;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Client;

public class LyroxClient : ILyroxClient
{
    private readonly IWorldDataManager _worldDataManager;
    private readonly IChatManager _chatManager;
    private readonly IMessageBus _messageBus;
    private readonly INetworkingManager _networkingManager;
    private readonly IPlayerManager _playerManager;

    public event EventHandler<ChatMessageReceivedMessage>? ChatMessageReceived;
    public event EventHandler<ConnectionEstablishedMessage>? Connected;
    public event EventHandler<ConnectionTerminatedMessage>? Disconnected;

    public LyroxClient(IWorldDataManager worldDataManager,
        IChatManager chatManager,
        IMessageBus messageBus,
        INetworkingManager networkingManager,
        IPlayerManager playerManager)
    {
        _worldDataManager = worldDataManager;
        _chatManager = chatManager;
        _messageBus = messageBus;
        _networkingManager = networkingManager;
        _playerManager = playerManager;

        SetupEvents();
    }

    private void SetupEvents()
    {
        _ = _messageBus.Subscribe<ConnectionEstablishedMessage>(
            (evt) => { Connected?.Invoke(this, evt); return Task.CompletedTask; });

        _ = _messageBus.Subscribe<ConnectionTerminatedMessage>(
            (evt) => { Disconnected?.Invoke(this, evt); return Task.CompletedTask; });

        _ = _messageBus.Subscribe<ChatMessageReceivedMessage>(
            (evt) => { ChatMessageReceived?.Invoke(this, evt); return Task.CompletedTask; });
    }

    public Task Connect()
        => _networkingManager.Connect();

    public Task SendChatMessage(string message)
        => _chatManager.SendChatMessage(message);

    public Task SendPrivateMessage(string message, string player)
        => _chatManager.SendPrivateMessage(message, player);

    public Task SendCommand(string command, string[] arguments)
        => _chatManager.SendCommand(command, arguments);

    public void Goto(Vector3d position)
        => _playerManager.Move(position);

    public BlockState? GetBlock(Vector3i position)
        => _worldDataManager.GetBlock(position);

    public ChunkSection? GetChunkSection(Vector3i chunkPos)
        => _worldDataManager.GetChunk(chunkPos.X, chunkPos.Z)?.Sections[chunkPos.Y];
}
