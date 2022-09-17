using Lyrox.Abstraction;
using Lyrox.Chat.Events;
using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Models.Chat;

namespace Lyrox
{
    public class LyroxClient : ILyroxClient
    {
        private readonly IWorldDataManager _worldDataManager;
        private readonly IChatManager _chatManager;
        private readonly IEventManager _eventManager;
        private readonly INetworkingManager _networkingManager;

        private readonly LyroxConfiguration _configuration;

        public event EventHandler<ChatMessage> ChatMessageReceived;

        public LyroxClient(IWorldDataManager worldDataManager, IChatManager chatManager, IEventManager eventManager, INetworkingManager networkingManager, LyroxConfiguration configuration)
        {
            _worldDataManager = worldDataManager;
            _chatManager = chatManager;
            _eventManager = eventManager;
            _networkingManager = networkingManager;
            _configuration = configuration;

            SetupEvents();
        }

        private void SetupEvents()
        {
            _eventManager.RegisterEventHandler<ChatMessageReceivedEvent>((evt)
                => ChatMessageReceived?.Invoke(this, evt.Message));
        }

        public async Task Connect()
            => await _networkingManager.Connect();

        public void SendChatMessage(string message, string receiver = "")
            => throw new NotImplementedException();
    }
}
