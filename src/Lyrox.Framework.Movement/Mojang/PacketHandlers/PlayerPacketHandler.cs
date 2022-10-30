using Lyrox.Framework.Core.Abstraction;
using Lyrox.Framework.Core.Networking.Abstraction.Packet.Handler;
using Lyrox.Framework.Player.Mojang.Packets.ClientBound;
using Lyrox.Framework.Player.Mojang.Packets.ServerBound;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Player.Mojang.PacketHandlers
{
    internal class PlayerPacketHandler : IPacketHandler<SynchronizePlayerPosition>
    {
        private readonly IPhysicsPlayer _physicsPlayer;
        private readonly INetworkingManager _networkingManager;

        public PlayerPacketHandler(IPhysicsPlayer physicsPlayer, INetworkingManager networkingManager)
        {
            _physicsPlayer = physicsPlayer;
            _networkingManager = networkingManager;
        }

        public async Task HandlePacket(SynchronizePlayerPosition networkPacket)
        {
            var flags = (SynchronizePlayerPosition.RelativeValues)networkPacket.Flags;
            var position = new Vector3d(flags.HasFlag(SynchronizePlayerPosition.RelativeValues.X) ? (_physicsPlayer.Position.X + networkPacket.X) : networkPacket.X,
                flags.HasFlag(SynchronizePlayerPosition.RelativeValues.Y) ? (_physicsPlayer.Position.Y + networkPacket.Y) : networkPacket.Y,
                flags.HasFlag(SynchronizePlayerPosition.RelativeValues.Z) ? (_physicsPlayer.Position.Z + networkPacket.Z) : networkPacket.Z);

            var rotation = new Rotation(flags.HasFlag(SynchronizePlayerPosition.RelativeValues.X_ROT) ? (_physicsPlayer.Rotation.Yaw + networkPacket.Yaw) : networkPacket.Yaw,
                flags.HasFlag(SynchronizePlayerPosition.RelativeValues.Y_ROT) ? (_physicsPlayer.Rotation.Pitch + networkPacket.Pitch) : networkPacket.Pitch);

            _physicsPlayer.SetCurrentPosition(position);
            _physicsPlayer.SetCurrentRotation(rotation);

            await _networkingManager.SendPacket(new ConfirmTeleportation(networkPacket.TeleportID));
        }
    }
}
