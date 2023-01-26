using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface IPlayerManager
{
    void BreakBlock(Vector3d blockPosition);
    void InteractWithBlock(Vector3d blockPosition);
    void Jump();
    void Move(Vector3d direction);
    void PlaceBlock(Vector3d blockPosition);
}
