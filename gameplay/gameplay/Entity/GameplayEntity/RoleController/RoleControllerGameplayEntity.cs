using Game.Core;
using GameFramework;
using GameFramework.Entity;

namespace Game.Gameplay
{

    /// <summary>
    /// @TEMP: 暂时无用，如果有复杂控制逻辑，不方便放在player或者具体role逻辑上时，可以使用这个controller
    /// </summary>
    public class RoleControllerGameplayEntity : GameplayEntity
    {
        //private IRoleController m_RoleController=> EntityLogic as IRoleController;

        public RoleControllerGameplayEntity(IEntity entity) : base(entity)
        {

        }

        public override void OnShow(EntityData entityData)
        {
            base.OnShow(entityData);

        }

        public override void OnAttachTo(IEntity parentEntity, object userData)
        {
            base.OnAttachTo(parentEntity, userData);

        }

        public override void OnDetachFrom(IEntity parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }
    }
}