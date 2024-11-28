//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Entity;
using UnityEngine;
using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public abstract class GameEntityLogic : EntityLogic
            , IGameplayTagOwner
            , IGameEntityLogic
    {
        [SerializeField]
        private List<EntityLogicSocketBase> m_LogicSockets = new List<EntityLogicSocketBase>(); 
            

        public GameplayEntity GameplayEntity { get; protected set; }

        
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
            

            CreateGameplayEntity();
            if (GameplayEntity == null)
            { //Log.Warning("GameplayEntity is null, check if should create in CreateGameplay function");
            }
            GameplayEntity?.OnInit(EntityData);
            //OnPreInitLogicSocket(EntityData);
            foreach (var entityLogicSocketBase in m_LogicSockets)
            {
                    entityLogicSocketBase.Init(this);
            }
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
            EntityData?.Clear();
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnShow(object userData)
#else
        public internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);
            GameplayEntity?.OnShow(EntityData);
            Name = Utility.Text.Format("[Entity {0}]", Id);
            CachedTransform.localPosition = EntityData.InitPosition.ToVector3();
            CachedTransform.localRotation = EntityData.InitRotation.ToQuaternion();
            CachedTransform.localScale = Vector3.one;
            
            foreach (var entityLogicSocketBase in m_LogicSockets)
            {
                    entityLogicSocketBase.OnShow();
            }
        }

#if UNITY_2017_3_OR_NEWER
            public override void OnHide(bool isShutdown, object userData)
#else
        public internal override void OnHide(bool isShutdown, object userData)
#endif
        {
            base.OnHide(isShutdown, userData);
            foreach (var entityLogicSocketBase in m_LogicSockets)
            {
                    entityLogicSocketBase.Hide(isShutdown);
            }
            GameplayEntity?.OnHide(isShutdown, EntityData);
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
            foreach (var entityLogicSocketBase in m_LogicSockets)
            {
                    entityLogicSocketBase.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        
        //logic socket
        public void RegisterLogicSocket(EntityLogicSocketBase socket)
        {
                if(!m_LogicSockets.Contains(socket))
                        m_LogicSockets.Add(socket);
        }

        public T FindLogicSocket<T>() where T : EntityLogicSocketBase
        {
                foreach (var logicSocket in m_LogicSockets)
                {
                        if (typeof(T) == logicSocket.GetType())
                        {
                                return (T)logicSocket;
                        }
                }

                return null;
        }
        
    }
}
