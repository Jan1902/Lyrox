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

        private readonly Dictionary<ProtocolState, Dictionary<int, Action<byte[]>>> _handlers;
        private ProtocolState _currentProtocolState;

        public MojangNetworkingPacketHandler(INetworkConnection networkConnection, IEventManager eventManager)
        {
            _networkConnection = networkConnection;
            _eventManager = eventManager;

            _currentProtocolState = ProtocolState.Handshaking;
            _eventManager.RegisterEventHandler<ProtocolStateChanged>(HandleProtocolStateChange);

            _handlers = new()
            {
                [ProtocolState.Login] = new()
                {
                    [0x00] = HandleDisconnectLogin,
                    [0x02] = HandleLoginSuccess
                }
            };
        }

        private void HandleProtocolStateChange(ProtocolStateChanged protocolStateChanged)
            => _currentProtocolState = protocolStateChanged.ProtocolState;

        public void HandlePacket(int opCode, byte[] data)
        {
            if (_handlers.ContainsKey(_currentProtocolState)
                && _handlers[_currentProtocolState].ContainsKey(opCode))
                _handlers[_currentProtocolState][opCode].Invoke(data);
        }

        private void HandleLoginSuccess(byte[] data)
            => _eventManager.PublishEvent(new ProtocolStateChanged(ProtocolState.Play));

        private void HandleDisconnectLogin(byte[] data)
            => _eventManager.PublishEvent(new ConnectionTerminatedEvent());

        public void HandlePacket(KeepAliveCB networkPacket)
            => _networkConnection.SendPacket(new KeepAliveSB(networkPacket.KeepAliveID));
    }
}
