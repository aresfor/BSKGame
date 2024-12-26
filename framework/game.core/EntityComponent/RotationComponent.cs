using Game.Math;

namespace Game.Core
{
    public class RotationComponent:BaseComponent
    {
        public quaternion Rotation;

        protected override void OnClear()
        {
            Rotation = default;
        }
    }
}

