using Game.Core;
using Game.Gameplay;
using Game.Math;

namespace Game.Client
{
    public partial class RoleEntityLogic : IView
    {
        public float3 ViewPosition
        {
            get =>Position.ToFloat3();
            set => Position = value.ToVector3();
        }

        public quaternion ViewRotation
        {
            get => Rotation.ToQuaternion();
            set => Rotation = value.ToQuaternion();
        }
    }
}