using Game.Core;
using GameFramework;
using GameFramework.Entity;

namespace Game.Gameplay;

public class RoleControllerGameplayEntity: GameplayEntity
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