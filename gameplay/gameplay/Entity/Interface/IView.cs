using Game.Math;

namespace Game.Gameplay;

public interface IView
{
    float3 ViewPosition { get; set; }
    quaternion ViewRotation { get; set; }
}