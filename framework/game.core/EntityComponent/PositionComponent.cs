using Game.Math;

namespace Game.Core;

public class PositionComponent:BaseComponent
{
    public float3 Position;
    protected override void OnClear()
    {
        Position = default;
    }
}