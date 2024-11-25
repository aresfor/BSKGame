//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Game.Core;
using Game.Gameplay;
using GameFramework.Entity;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体逻辑基类。
    /// </summary>
    public abstract class EntityLogic : BaseMonoBehaviour, IEntityLogic
    {
        private bool m_Available = false;
        private bool m_Visible = false;
        private Entity m_Entity = null;
        private int m_OriginalLayer = 0;
        private Transform m_OriginalParentTransform;
        
        
        public EntityData EntityData { get; private set; }

        
        public void AddTag(string tag)
        {
            EntityData.AddTag(tag);
        }

        public void RemoveTag(string tag)
        {
            EntityData.RemoveTag(tag);
        }

        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact)
        {
            return EntityData.HasTag(tag, checkType);
        }

        public void ClearAllTag()
        {
            EntityData.ClearAllTag();
        }
        
        /// <summary>
        /// 获取实体。
        /// </summary>
        public Entity Entity
        {
            get
            {
                return m_Entity;
            }
        }

        public IEntity EntityInterface { get=> m_Entity; }

        /// <summary>
        /// 获取或设置实体名称。
        /// </summary>
        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;
            }
        }

        /// <summary>
        /// 获取实体是否可用。
        /// </summary>
        public bool Available
        {
            get
            {
                return m_Available;
            }
        }

        /// <summary>
        /// 获取或设置实体是否可见。
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Available && m_Visible;
            }
            set
            {
                if (!m_Available)
                {
                    Log.Warning("Entity '{0}' is not available.", Name);
                    return;
                }

                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnInit(object userData)
        {
            EntityData = userData as EntityData;
            if (EntityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }
            m_Entity = GetComponent<Entity>();
            m_OriginalLayer = gameObject.layer;
            m_OriginalParentTransform = CachedTransform.parent;
        }
        
        /// <summary>
        /// 实体回收。
        /// </summary>
        public virtual void OnRecycle()
        {
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnShow(object userData)
        {
            m_Available = true;
            Visible = true;
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnHide(bool isShutdown, object userData)
        {
            gameObject.SetLayerRecursively(m_OriginalLayer);
            Visible = false;
            m_Available = false;
        }

        


        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnAttached(IEntity childEntity, Transform parentTransform, object userData)
        {
            
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnDetached(IEntity childEntity, object userData)
        {
        }

        

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnAttachTo(IEntity parentEntity, Transform parentTransform, object userData)
        {
            //CachedTransform.SetParent(parentTransform);
        }


        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnDetachFrom(IEntity parentEntity, object userData)
        {
            //CachedTransform.SetParent(m_OriginalParentTransform);
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 设置实体的可见性。
        /// </summary>
        /// <param name="visible">实体的可见性。</param>
        public void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
        

        }
    
}
