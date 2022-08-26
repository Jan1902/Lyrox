using Lyrox.Core.Events.Abstraction;
using Lyrox.Networking.Packets.ClientBound;
using Lyrox.Networking.Types;
using Microsoft.Extensions.Logging;

namespace Lyrox.Networking.Packets
{
    internal class PacketHandler : IPacketHandler
    {
        private readonly IEventManager _eventManager;
        private readonly ILogger<PacketHandler> _logger;

        private readonly Dictionary<ProtocolState, Dictionary<int, Type>> _packetTypes;

        private ProtocolState _currentProtocolState;

        public PacketHandler(IEventManager eventManager, ILogger<PacketHandler> logger)
        {
            _eventManager = eventManager;
            _logger = logger;

            _currentProtocolState = ProtocolState.Handshaking;

            _packetTypes = new();
            SetupMapping();
        }

        public void SetProtocolState(ProtocolState protocolState)
            => _currentProtocolState = protocolState;

        private void SetupMapping()
        {
            foreach (var protocolState in Enum.GetValues<ProtocolState>())
                _packetTypes[protocolState] = new();

            _packetTypes[ProtocolState.Login][(int)OPLogin.LoginSuccess] = typeof(LoginSuccess);
            _packetTypes[ProtocolState.Login][(int)OPLogin.Disconnect] = typeof(DisconnectLogin);
            _packetTypes[ProtocolState.Play][(int)OPPlay.KeepAliveCB] = typeof(KeepAlive);
        }

        public void HandlePacket(int opCode, byte[] data)
        {
            if (!_packetTypes[_currentProtocolState].ContainsKey(opCode))
            {
                _logger.LogWarning("Received unknown packet with OP Code {op} and length {length}", opCode, data.Length);
                return;
            }

            var packetType = _packetTypes[_currentProtocolState][opCode];
            var packet = Activator.CreateInstance(packetType) as ClientBoundPacket;
            packet.AddInitialBytes(data);
            packet.Parse();

            if (packetType.IsAssignableTo(typeof(IMappedToEvent)))
                _eventManager.PublishEvent(((IMappedToEvent)packet).GetEvent()); //TODO: FIX THIS: TYPE RETURNED IS EVENT, SPECIFIC TYPE IS NEEDED FOR HANDLER RESOLUTION
        }
    }
}
