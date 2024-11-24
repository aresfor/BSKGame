
using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityEngine;

//@TEMP:
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField] private TextAsset m_GameplayTagTextAsset = null;

        private List<IUpdateableUtility> m_UpdateableUtilities = new List<IUpdateableUtility>();
        private List<IShutdownUtility> m_ShutdownUtilities = new List<IShutdownUtility>();
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            RegisterListener();
            RegisterUtilities();

            InitializeGameplayTag();
        }
        
        /// <summary>
        /// 初始化实用类，之后直接全局访问
        /// </summary>
        private void RegisterUtilities()
        {
            //@TEMP
            ResourceExtension.Initialize();
            
            PhysicsUtils.Initialize(new UnityPhysicsUtility());
            var unityVfxUtility = new UnityVFXUtility();
            VFXUtils.Initialize(unityVfxUtility);
            m_UpdateableUtilities.Add(unityVfxUtility);
            
        }

        private void RegisterListener()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, EntityExtension.OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, EntityExtension.OnShowEntityFail);
        }

        private void UnRegisterListener()
        {
            //游戏关闭时事件系统自动清理
            //GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, EntityExtension.OnShowEntitySuccess);
            //GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, EntityExtension.OnShowEntityFail);
        }

        private void InitializeGameplayTag()
        {
            if (m_GameplayTagTextAsset == null)
            {
                Log.Error("GameplayTag Text Asset is null, check");
                return;
            }
            GameplayTagTree gameplayTagTree = UnityGameplayTagExtension.InitializeGameplayTag(m_GameplayTagTextAsset.text);
            //@TEMP:
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            if (gameplayTagTree == null)
            {
                Log.Warning("Parse GameplayTag failure.");
                return;
            }
        }

        //@TEMP:
        private void Update()
        {
            foreach (var updateableUtility in m_UpdateableUtilities)
            {
                updateableUtility.Update(Time.deltaTime);
            }
        }

        //@TEMP:
        private void OnDestroy()
        {
            foreach (var shutdownUtility in m_ShutdownUtilities)
            {
                shutdownUtility.Shutdown();
            }
            UnRegisterListener();
            m_UpdateableUtilities.Clear();
            m_ShutdownUtilities.Clear();
        }
    }
}
