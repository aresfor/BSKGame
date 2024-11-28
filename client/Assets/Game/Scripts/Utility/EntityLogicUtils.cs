using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public static  class EntityLogicUtils
    {
        public static float3 GetPosition(IEntityLogic entityLogic)
        {
            if (entityLogic is IView view)
                return view.ViewPosition;

            return entityLogic.EntityInterface.GetComponent<PositionComponent>().Position;
        }
        
        public static quaternion GetRotation(IEntityLogic entityLogic)
        {
            if (entityLogic is IView view)
                return view.ViewRotation;

            return entityLogic.EntityInterface.GetComponent<RotationComponent>().Rotation;
        }

        
        //@TODO: 移动到Gameplay中去
        public static void ApplyDamageTo(ApplyDamageInfo damageInfo)
        {
            var targetEntity = GameEntry.Entity.GetEntity(damageInfo.TargetEntityId);
            if (targetEntity == null)
            {
                Log.Error("伤害目标为空, check");
                return;
            }
            var targetEntityData = targetEntity.Logic.EntityData;
            float currentHealth = targetEntityData.GetProperty(EPropertyDefine.Health);
            targetEntityData.SetProperty(EPropertyDefine.Health, currentHealth - damageInfo.DamageNum);
            
            //调用链
            if (targetEntity.Logic is IGameEntityLogic gameEntityLogic)
            {
                gameEntityLogic.GameplayEntity.ReceiveDamage(damageInfo);
                //如果死了
                if (targetEntityData.GetProperty(EPropertyDefine.Health) <= 0)
                {
                    targetEntity.AddTag(Constant.GameplayTag.Dead);
                    gameEntityLogic.GameplayEntity.Kill(damageInfo);
                }
            }
            
            
            ReferencePool.Release(damageInfo);
        }
    }
}