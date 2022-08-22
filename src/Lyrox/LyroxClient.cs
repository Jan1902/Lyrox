using Lyrox.Abstraction;
using Lyrox.Core.Abstraction;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events;

namespace Lyrox
{
    public class LyroxClient : ILyroxClient
    {
        private readonly IWorldDataManager _worldDataManager;
        private readonly IChatManager _chatManager;
        private readonly IEventManager _eventManager;
        private readonly INetworkingManager _networkingManager;

        private readonly LyroxConfiguration _configuration;

        public LyroxClient(IWorldDataManager worldDataManager, IChatManager chatManager, IEventManager eventManager, INetworkingManager networkingManager, LyroxConfiguration configuration)
        {
            _worldDataManager = worldDataManager;
            _chatManager = chatManager;
            _eventManager = eventManager;
            _networkingManager = networkingManager;
            _configuration = configuration;
        }

        public async Task Connect()
            => await _networkingManager.Connect();
    }
}
