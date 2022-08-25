using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Core.Events;
using Lyrox.Networking.Packets.ClientBound;
using Lyrox.Networking.Packets.ServerBound;
using Lyrox.Networking.Types;
using Microsoft.Extensions.Logging;

namespace Lyrox.Networking.Packets
{
    internal class PacketHandler : IPacketHandler
    {
        private readonly IEventManager _eventManager;
        private readonly ILogger<PacketHandler> _logger;

        private readonly Dictionary<int, Type> _packets;

        public PacketHandler(IEventManager eventManager, ILogger<PacketHandler> logger)
        {
            _eventManager = eventManager;
            _logger = logger;

            _packets = new();
            SetupMapping();
        }

        private void SetupMapping()
        {
            _packets[(int)OPHandshaking.Handshake] = typeof(Handshake);
        }

        public void HandlePacket(int opCode, byte[] data)
        {
            if (!_packets.ContainsKey(opCode))
            {
                _logger.LogWarning("Received unknown packet with OP Code {op} and length {lengnth}", opCode, data.Length);
                return;
            }

            var packet = Activator.CreateInstance(_packets[opCode]) as ClientBoundPacket;
            packet.AddBytes(data);
            packet.Parse();

            if (_packets[opCode].IsAssignableTo(typeof(IMappedToEvent)))
                _eventManager.PublishEvent(((IMappedToEvent)packet).GetEvent());
        }
    }
}
