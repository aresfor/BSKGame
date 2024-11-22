using Game.Core;
using Game.Gameplay;
using GameFramework.Entity;
using UnityGameFramework.Runtime;


namespace Game.Client
{
    public abstract class EntityLogicSocketBase:BaseMonoBehaviour
    {
        public EntityLogic EntityLogic { get; private set; }
        public Entity Entity => EntityLogic.Entity;
        public EntityData EntityData => ((GameEntityLogic)EntityLogic)?.EntityData;
        public GameplayEntity GameplayEntity=>((GameEntityLogic)EntityLogic)?.GameplayEntity;

        public void Init(EntityLogic entityLogic)
        {
            EntityLogic = entityLogic;
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        public void Recycle()
        {
            OnRecycle();
            EntityLogic = null;
        }

        protected virtual void OnRecycle()
        {
            
        }

        public virtual void OnShow()
        {
            
        }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        public virtual void OnHide(bool isShutdown)
        {
            
        }

        public virtual void InternalSetVisible(bool visible)
        {
            
        }
        
        /// <summary>
        /// 更换模型需要调用
        /// </summary>
        public virtual void Reset()
        {
            
        }
    }
}