using Game.Core;
using GameFramework.Entity;

namespace Game.Gameplay
{

    public class PlayerGameplayEntity : GameplayEntity
    {
        public PlayerGameplayEntity(IEntity entity) : base(entity)
        {
        }

        public override void OnInit(EntityData entityData)
        {

        }

        public override void OnRecycle()
        {

        }

        public override void OnShow(EntityData entityData)
        {

        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            //GameFramework.GameFrameworkLog.Info("PlayerGameplayEntity Update!");
        }

    }
}