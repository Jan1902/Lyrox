using Lyrox.Framework.Core.Events.Abstraction;
using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang.Events;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;
using Lyrox.Framework.Networking.Mojang.Packets.ServerBound;
using Lyrox.Framework.Networking.Mojang.Types;

namespace Lyrox.Framework.Networking.Mojang.PacketHandlers
{
    public class MojangNetworkingPacketHandler : IRawPacketHandler, IPacketHandler<KeepAliveCB>
    {
        private readonly INetworkConnection _networkConnection;
        private readonly IEventManager _eventManager;

        private readonly Dictionary<ProtocolState, Dictionary<int, Func<byte[], Task>>> _handlers;
        private ProtocolState _currentProtocolState;

        public MojangNetworkingPacketHandler(INetworkConnection networkConnection, IEventManager eventManager)
        {
            _networkConnection = networkConnection;
            _eventManager = eventManager;

            _currentProtocolState = ProtocolState.Handshaking;
            _eventManager.RegisterEventHandler<ProtocolStateChangedEvent>(HandleProtocolStateChange);

            _handlers = new()
            {
                [ProtocolState.Login] = new()
                {
                    [0x00] = HandleDisconnectLogin,
                    [0x02] = HandleLoginSuccess
                }
            };
        }

        private void HandleProtocolStateChange(ProtocolStateChangedEvent protocolStateChanged)
            => _currentProtocolState = protocolStateChanged.ProtocolState;

        public async Task HandleRawPacket(int opCode, byte[] data)
        {
            if (_handlers.ContainsKey(_currentProtocolState)
                && _handlers[_currentProtocolState].ContainsKey(opCode))
                await _handlers[_currentProtocolState][opCode].Invoke(data);
        }

        private async Task HandleLoginSuccess(byte[] data)
            => await _eventManager.PublishEvent(new ProtocolStateChangedEvent(ProtocolState.Play));

        private async Task HandleDisconnectLogin(byte[] data)
            => await _eventManager.PublishEvent(new ConnectionTerminatedEvent());

        public async Task HandlePacket(KeepAliveCB networkPacket)
            => await _networkConnection.SendPacket(new KeepAliveSB(networkPacket.KeepAliveID));
    }
}
