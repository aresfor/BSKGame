//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Entity;
using System;
using System.Collections.Generic;
using Game.Core;
using GameFramework.Runtime;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体。
    /// </summary>
    public sealed class Entity : MonoBehaviour, IEntity
    {
        private int m_Id;
        private string m_EntityAssetName;
        private IEntityGroup m_EntityGroup;
        private EntityLogic m_EntityLogic;

        public Dictionary<Type, IComponent> Components { get; private set; }
            = new Dictionary<Type, IComponent>();

        public T GetComponent<T>() where T : class, IComponent
        {
            if (Components.ContainsKey(typeof(T)))
            {
                return Components[typeof(T)] as T;
            }

            return null;
        }
        
        public T AddComponent<T>(T component) where T : class, IComponent
        {
            Type type = typeof(T);
            Components[type] = component;
            return component;
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            if (Components.ContainsKey(typeof(T)))
            {
                var comp = Components[typeof(T)];
                Components.Remove(typeof(T));
                if (comp != null && comp is IReference referenceComp)
                {
                    ReferencePool.Release(referenceComp);
                }
            }

            return;
        }

        public void RemoveAllComponent()
        {
            foreach (var component in Components.Values)
            {
                if (component is IReference referenceComp)
                {
                    ReferencePool.Release(referenceComp);
                }
            }

            Components.Clear();
        }


        /// <summary>
        /// 获取实体逻辑接口。
        /// </summary>
        public IEntityLogic LogicInterface
        {
            get => m_EntityLogic;
        }

        /// <summary>
        /// 获取实体逻辑。
        /// </summary>
        public EntityLogic Logic
        {
            get => m_EntityLogic;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get { return m_Id; }
        }

        public string Name
        {
            get => gameObject.name;
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get { return m_EntityAssetName; }
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        public object Handle
        {
            get { return gameObject; }
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup
        {
            get { return m_EntityGroup; }
        }


        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance,
            object userData)
        {
            m_Id = entityId;
            m_EntityAssetName = entityAssetName;
            if (isNewInstance)
            {
                m_EntityGroup = entityGroup;
            }
            else if (m_EntityGroup != entityGroup)
            {
                Log.Error("Entity group is inconsistent for non-new-instance entity.");
                return;
            }

            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            //@注意：不再通过type来获取和创建了 直接在预制体上手动添加
            // Type entityLogicType = showEntityInfo.EntityLogicType;
            // if (entityLogicType == null)
            // {
            //     Log.Error("Entity logic type is invalid.");
            //     return;
            // }

            // if (m_EntityLogic != null)
            // {
            //     if (m_EntityLogic.GetType() == entityLogicType)
            //     {
            //         m_EntityLogic.enabled = true;
            //         return;
            //     }
            //
            //     Destroy(m_EntityLogic);
            //     m_EntityLogic = null;
            // }

            m_EntityLogic = (EntityLogic)gameObject.GetComponent(typeof(EntityLogic));


            //m_EntityLogic = gameObject.AddComponent(entityLogicType) as EntityLogic;
            if (m_EntityLogic == null)
            {
                Log.Error("Entity '{0}' can not add entity logic.", entityAssetName);
                return;
            }

            //让unity捕获而不是try catch
            m_EntityLogic.OnInit(showEntityInfo.UserData);

            try
            {
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnInit with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        public void OnRecycle()
        {
            try
            {
                m_EntityLogic.OnRecycle();
                m_EntityLogic.enabled = false;
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnRecycle with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }

            m_Id = 0;
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void OnShow(object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            try
            {
                m_EntityLogic.OnShow(showEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnShow with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnHide(bool isShutdown, object userData)
        {
            try
            {
                m_EntityLogic.OnHide(isShutdown, userData);
                RemoveAllComponent();
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnHide with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttached(IEntity childEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttached(childEntity, attachEntityInfo.ParentTransform, attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttached with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetached(IEntity childEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetached(childEntity, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetached with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttachTo(IEntity parentEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttachTo(parentEntity, attachEntityInfo.ParentTransform, attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttachTo with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }

            ReferencePool.Release(attachEntityInfo);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetachFrom(IEntity parentEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetachFrom(parentEntity, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetachFrom with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                m_EntityLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnUpdate with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }
    }
}