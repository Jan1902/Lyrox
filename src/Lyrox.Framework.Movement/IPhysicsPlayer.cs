using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Player
{
    internal interface IPhysicsPlayer
    {
        Vector3d Position { get; }
        Rotation Rotation { get; }
        bool IsOnGround { get; }

        void Jump();
        void LookAt(Vector3d position);
        void SetCurrentPosition(Vector3d position);
        void SetCurrentRotation(Rotation rotation);
        void SetMovementGoal(Vector3d goal);
        void SetSprinting(bool sprinting);
    }
}
