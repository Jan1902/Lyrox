using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Player.Mojang.Packets.ServerBound;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Player;

internal class PlayerManager : IPlayerManager
{
    private readonly IPhysicsPlayer _physicsPlayer;
    private readonly INetworkingManager _networkingManager;

    private readonly Timer _updateTimer;
    private const int UpdateInterval = 20;

    public PlayerManager(IPhysicsPlayer physicsPlayer, INetworkingManager networkingManager)
    {
        _physicsPlayer = physicsPlayer;
        _networkingManager = networkingManager;

        _updateTimer = new Timer(new TimerCallback((o) => SendPositionUpdate()), null, UpdateInterval, UpdateInterval);
    }

    private void SendPositionUpdate()
    {
        if (_physicsPlayer.Position is null
            || _physicsPlayer.Rotation is null)
            return;

        _networkingManager.SendPacket(new SetPlayerPositionAndRotation(
                _physicsPlayer.Position,
                _physicsPlayer.Rotation,
                _physicsPlayer.IsOnGround));
    }

    public void Jump() => _physicsPlayer.Jump();

    public void Move(Vector3d direction)
    {
        //TEMPORARY
        _physicsPlayer.SetMovementGoal(direction);
        _physicsPlayer.LookAt(direction + new Vector3d(0, 1.5, 0));
    }

    public void BreakBlock(Vector3d blockPosition)
        => throw new NotImplementedException();

    public void PlaceBlock(Vector3d blockPosition)
        => throw new NotImplementedException();

    public void InteractWithBlock(Vector3d blockPosition)
        => throw new NotImplementedException();
}
