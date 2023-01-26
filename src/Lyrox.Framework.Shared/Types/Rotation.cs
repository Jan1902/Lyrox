namespace Lyrox.Framework.Shared.Types;

public class Rotation
{
    public float Pitch { get; set; }
    public float Yaw { get; set; }

    public Rotation(float pitch, float yaw)
    {
        Pitch = pitch;
        Yaw = yaw;
    }
}
