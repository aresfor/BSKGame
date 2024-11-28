using Game.Core;
using Game.Gameplay;
using Game.Math;

namespace Game.Client
{
    public partial class RoleEntityLogic : IView
    {
        //@TODO: 让system管理
        public float3 ViewPosition
        {
            get =>Position.ToFloat3();
            set
            {
                Position = value.ToVector3();
                Entity.GetComponent<PositionComponent>().Position = Position.ToFloat3();
            }
        }
        
        //@TODO: 让system管理
        public quaternion ViewRotation
        {
            get => Rotation.ToQuaternion();
            set
            {
                Rotation = value.ToQuaternion();
                Entity.GetComponent<RotationComponent>().Rotation = Rotation.ToQuaternion();

            }
        }
    }
}