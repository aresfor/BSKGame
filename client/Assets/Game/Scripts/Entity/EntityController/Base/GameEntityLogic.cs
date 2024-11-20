//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Entity;
using UnityEngine;
using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public abstract class GameEntityLogic : EntityLogic, IGameplayTagOwner
    {
    
        [SerializeField]
        private EntityData m_EntityData = null;

        protected GameplayEntity GameplayEntity { get; set; }
        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        public Animation CachedAnimation
        {
            get;
            private set;
        }

        protected virtual void CreateGameplayEntity(){}
        
#if UNITY_2017_3_OR_NEWER
            public override void OnInit(object userData)
#else
        public internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            m_EntityData = userData as EntityData;
            if (m_EntityData == null)
            {
                    Log.Error("Entity data is invalid.");
                    return;
            }

            CreateGameplayEntity();
            if (GameplayEntity == null)
            { //Log.Warning("GameplayEntity is null, check if should create in CreateGameplay function");
            }
            GameplayEntity?.OnInit(m_EntityData);
            CachedAnimation = GetComponent<Animation>();
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnRecycle()
#else
        public internal override void OnRecycle()
#endif
        {
            base.OnRecycle();
            GameplayEntity?.OnRecycle();
            m_EntityData?.Clear();
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnShow(object userData)
#else
        public internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);
            GameplayEntity?.OnShow(m_EntityData);
            Name = Utility.Text.Format("[Entity {0}]", Id);
            CachedTransform.localPosition = m_EntityData.Position.ToVector3();
            CachedTransform.localRotation = m_EntityData.Rotation.ToQuaternion();
            CachedTransform.localScale = Vector3.one;
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnHide(bool isShutdown, object userData)
#else
        public internal override void OnHide(bool isShutdown, object userData)
#endif
        {
            base.OnHide(isShutdown, userData);
            GameplayEntity?.OnHide(isShutdown, m_EntityData);
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnAttached(IEntity childEntity, Transform parentTransform, object userData)
#else
        public internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttached(childEntity, parentTransform, userData);
                GameplayEntity?.OnAttached(childEntity, userData);
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnDetached(IEntity childEntity, object userData)
#else
        public internal override void OnDetached(EntityLogic childEntity, object userData)
#endif
        {
            base.OnDetached(childEntity, userData);
            GameplayEntity?.OnDetached(childEntity, userData);

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttachTo(IEntity parentEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
#endif
        {

            base.OnAttachTo(parentEntity, parentTransform, userData);
            GameplayEntity?.OnAttached(parentEntity, userData);
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnDetachFrom(IEntity parentEntity, object userData)
#else
        public internal override void OnDetachFrom(EntityLogic parentEntity, object userData)
#endif
        {
            base.OnDetachFrom(parentEntity, userData);
            GameplayEntity?.OnDetachFrom(parentEntity, userData);

        }

#if UNITY_2017_3_OR_NEWER
        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        public internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            GameplayEntity?.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public void AddTag(string tag)
        {
                m_EntityData.AddTag(tag);
        }

        public void RemoveTag(string tag)
        {
                m_EntityData.RemoveTag(tag);
        }

        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact)
        {
                return m_EntityData.HasTag(tag, checkType);
        }

        public void ClearAllTag()
        {
                m_EntityData.ClearAllTag();
        }
    }
}
