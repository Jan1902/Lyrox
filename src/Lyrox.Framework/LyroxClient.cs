using Lyrox.Framework.Abstraction;
using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Player;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework
{
    public class LyroxClient : ILyroxClient
    {
        private readonly IWorldDataManager _worldDataManager;
        private readonly IChatManager _chatManager;
        private readonly IEventManager _eventManager;
        private readonly INetworkingManager _networkingManager;
        private readonly IPlayerManager _playerManager;

        private readonly LyroxConfiguration _configuration;

        public event EventHandler<ChatMessageReceivedEvent> ChatMessageReceived;

        public LyroxClient(IWorldDataManager worldDataManager, IChatManager chatManager, IEventManager eventManager, INetworkingManager networkingManager, IPlayerManager playerManager, LyroxConfiguration configuration)
        {
            _worldDataManager = worldDataManager;
            _chatManager = chatManager;
            _eventManager = eventManager;
            _networkingManager = networkingManager;
            _playerManager = playerManager;
            _configuration = configuration;

            SetupEvents();
        }

        private void SetupEvents()
        {
            _eventManager.RegisterEventHandler<ChatMessageReceivedEvent>((evt)
                => ChatMessageReceived?.Invoke(this, evt));
        }

        public async Task Connect()
            => await _networkingManager.Connect();

        public void SendChatMessage(string message)
            => _chatManager.SendChatMessage(message);

        public void SendPrivateMessage(string message, string player)
            => _chatManager.SendPrivateMessage(message, player);

        public void SendCommand(string command, string[] arguments)
            => _chatManager.SendCommand(command, arguments);

        public void Goto(Vector3d position)
            => _playerManager.Move(position);

        public BlockState? GetBlock(Vector3i position)
            => _worldDataManager.GetBlock(position);

        public ChunkSection? GetChunkSection(Vector3i chunkPos)
            => _worldDataManager.GetChunk(chunkPos.X, chunkPos.Z)?.Sections[chunkPos.Y];
    }
}
