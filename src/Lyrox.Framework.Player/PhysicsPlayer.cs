using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Player;

internal class PhysicsPlayer : IPhysicsPlayer
{
    public Vector3d Position { get; private set; }
    public Rotation Rotation { get; private set; }

    private Vector3d _currentGoal;
    private Vector3d _currentVelocity = new Vector3d(0, 0, 0);

    private bool _shouldJump;
    private bool _isSprinting;

    private readonly Timer _interpolationTimer;

    private const int InterpolationInterval = 20;

    private const double Gravity = -32;
    private const double JumpHeight = 1.25;
    private const double WalkingSpeed = 4.317;
    private const double SprintingSpeed = 5.612;

    private readonly IWorldDataManager _worldDataManger;

    public PhysicsPlayer(IWorldDataManager worldDataManager)
    {
        _worldDataManger = worldDataManager;

        _interpolationTimer = new Timer(new TimerCallback((o) => Interpolate()), null, InterpolationInterval, InterpolationInterval);
    }

    private void Interpolate()
    {
        if (Position is null)
            return;

        if (IsOnGround && _currentVelocity.Y < 0)
            _currentVelocity.Y = 0;

        if (!IsOnGround)
            _currentVelocity.Y += Gravity * InterpolationInterval / 1000;

        if (IsOnGround && _shouldJump)
        {
            _currentVelocity.Y = Math.Sqrt(JumpHeight * -3 * Gravity);
            _shouldJump = false;
        }

        if (_currentGoal is not null)
        {
            var delta = _currentGoal - Position;
            var speed = (_isSprinting ? SprintingSpeed : WalkingSpeed) * InterpolationInterval / 1000;
            _currentVelocity.X = delta.X > .1 ? speed : delta.X < -.1 ? -speed : 0;
            _currentVelocity.Z = delta.Z > .1 ? speed : delta.Z < -.1 ? -speed : 0;
        }

        Position += _currentVelocity;
    }

    public bool IsOnGround
        => Position is null ? true
            : Position.Y == Math.Round(Position.Y)
                && (_worldDataManger.GetBlock(
                        (int)Math.Round(Position.X - .5),
                        (int)Position.Y - 1,
                        (int)Math.Round(Position.Z - .5))?
                    .IsSolid() ?? false);

    public void SetMovementGoal(Vector3d goal)
        => _currentGoal = goal;

    public void Jump()
        => _shouldJump = true;

    public void SetCurrentPosition(Vector3d position)
        => Position = position;

    public void SetCurrentRotation(Rotation rotation)
        => Rotation = rotation;

    public void LookAt(Vector3d position)
        => Rotation = Vector3d.Angle(Position + new Vector3d(0, 1.6, 0), position);

    public void SetSprinting(bool sprinting)
        => _isSprinting = sprinting;
}
