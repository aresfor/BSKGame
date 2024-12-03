
using System;
using System.Collections.Generic;
using Game.Client.Build.Rutime;
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
        private BuildInfo m_BuildInfo = null;
        [SerializeField]
        private UIUpdateResourceForm m_UpdateResourceFormTemplate = null;
        public BuildInfo BuildInfo=> m_BuildInfo;
        public UIUpdateResourceForm UpdateResourceFormTemplate => m_UpdateResourceFormTemplate;
        
        private List<IUpdateable> m_UpdateableUtilities = new List<IUpdateable>();
        private List<IShutdown> m_ShutdownUtilities = new List<IShutdown>();

        private LinkedList<IUpdateable> m_UpdatableSystems = new LinkedList<IUpdateable>();
        private LinkedList<ISystem> m_Systems = new LinkedList<ISystem>();
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            InitializeGameplayTag();
            
            RegisterListener();
            RegisterUtilities();
            
        }
        
        /// <summary>
        /// 初始化实用类，之后直接全局访问
        /// </summary>
        private void RegisterUtilities()
        {
            //@TEMP
            ResourceExtension.Initialize();
            TimeUtils.Initialize(new UnityTimeUtility());
            var unityInputPC = new UnityInputUtility_PC();
            m_UpdateableUtilities.Add(unityInputPC);
            InputUtils.Initialize(unityInputPC);
            PhysicsUtils.Initialize(new UnityPhysicsUtility());
            var unityVfxUtility = new UnityVFXUtility();
            VFXUtils.Initialize(unityVfxUtility);
            m_UpdateableUtilities.Add(unityVfxUtility);
            
        }

        public void AddSystem(ISystem system)
        {
            if (m_Systems.Contains(system))
                return;
            
            m_Systems.AddLast(system);
            if (system is IUpdateable updateSystem)
            {
                m_UpdatableSystems.AddLast(updateSystem);
            }
        }

        public void RemoveSystem(ISystem system)
        {
            m_Systems.Remove(system);
            
            if (system is IUpdateable updateSystem)
            {
                m_UpdatableSystems.Remove(updateSystem);
            }
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
            //更新utility
            foreach (var updateableUtility in m_UpdateableUtilities)
            {
                updateableUtility.Update(TimeUtils.DeltaTime());
            }
            
            //更新系统
            foreach (var updatableSystem in m_UpdatableSystems)
            {
                updatableSystem.Update(TimeUtils.DeltaTime());
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
            m_Systems.Clear();
            m_UpdatableSystems.Clear();
            m_UpdateableUtilities.Clear();
            m_ShutdownUtilities.Clear();
        }
    }
}
