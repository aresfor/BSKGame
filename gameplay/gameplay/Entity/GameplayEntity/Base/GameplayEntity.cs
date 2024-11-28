using Game.Core;
using GameFramework;
using GameFramework.Entity;


namespace Game.Gameplay;

public abstract class GameplayEntity
{
    private IEntity m_Entity;

    private EntityData m_EntityData;

    public IEntity Entity => m_Entity;
    public EntityData EntityData => m_EntityData;
    public IEntityLogic EntityLogic => m_Entity.LogicInterface;

    public GameplayEntity(IEntity entity)
    {
        m_Entity = entity;
    }

    public virtual void OnInit(EntityData entityData)
    {
        m_EntityData = entityData;
        //添加一些通用组件
        var positionComponent = ReferencePool.Acquire<PositionComponent>();
        positionComponent.Position = entityData.InitPosition;
        var rotationComponent = ReferencePool.Acquire<RotationComponent>();
        rotationComponent.Rotation = entityData.InitRotation;
        Entity.AddComponent(positionComponent);
        Entity.AddComponent(rotationComponent);
    }

    public void Kill(ApplyDamageInfo damageInfo)
    {
        OnKilled(damageInfo);
        var targetEntity = GameFrameworkEntry.GetModule<IEntityManager>().GetEntity(damageInfo.TargetEntityId);
        if (targetEntity.LogicInterface is IDamageable damageable)
        {
            damageable.OnKilled(damageInfo);
        }
    }

    public void ReceiveDamage(ApplyDamageInfo damageInfo)
    {
        OnReceiveDamage(damageInfo);
        var targetEntity = GameFrameworkEntry.GetModule<IEntityManager>().GetEntity(damageInfo.TargetEntityId);
        if (targetEntity.LogicInterface is IDamageable damageable)
        {
            damageable.OnTakenDamage(damageInfo);
        }
    }
    
    protected virtual void OnKilled(ApplyDamageInfo damageInfo){}
    protected virtual void OnReceiveDamage(ApplyDamageInfo damageInfo){}
    public virtual void OnRecycle(){}

    public virtual void OnShow(EntityData entityData){}

    public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds){}

    public virtual void OnHide(bool isShutdown, EntityData entityData){}

    public virtual void OnAttached(IEntity childEntity, object userData){}

    public virtual void OnDetached(IEntity childEntity, object userData){}

    public virtual void OnAttachTo(IEntity parentEntity, object userData){}

    public virtual void OnDetachFrom(IEntity parentEntity, object userData){}

}