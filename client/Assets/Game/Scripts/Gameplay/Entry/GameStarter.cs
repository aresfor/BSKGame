﻿using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{


    public class GameStarter:MonoBehaviour
    {
            
        private TempGameState m_GameState;
        
        
        private List<IUpdateable> m_UpdateableUtilities = new List<IUpdateable>();
        private List<IShutdown> m_ShutdownUtilities = new List<IShutdown>();

        private LinkedList<IUpdateable> m_UpdatableSystems = new LinkedList<IUpdateable>();
        private LinkedList<ISystem> m_Systems = new LinkedList<ISystem>();
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitGame();
        }

        private void Start()
        {
            StartGame(null);
        }
        
        private void InitGame()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
            Application.lowMemory += OnLowMemory;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            InitializeGameplayTag();

            
            //@TODO: gameplay相关的放到游戏启动逻辑中去
            RegisterListener();
            RegisterUtilities();
        }
        
        public void StartGame(object[] objects)
        {
            //@TODO:局内用状态机逻辑接管，目前先简单处理
            m_GameState = new TempGameState();
            m_GameState.EnterMenu();
            
            Log.Info("Hello StartGame Success!");
            Log.Info("HotUpdate Success!==!");

            GameplayTest test = new GameplayTest();
            test.GetNumber();

        }
        private void RegisterListener()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, EntityExtension.OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, EntityExtension.OnShowEntityFail);
        }
        public void InitializeGameplayTag()
        {
            var tagAsset = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>().GameplayTagTextAsset;
            if (tagAsset == null)
            {
                Log.Error("GameplayTag Text Asset is null, check");
                return;
            }
            
            GameplayTagTree gameplayTagTree = UnityGameplayTagExtension.InitializeGameplayTag(tagAsset.text);
            //@TEMP:
// #if UNITY_EDITOR
//             AssetDatabase.Refresh();
// #endif
            if (gameplayTagTree == null)
            {
                Log.Warning("Parse GameplayTag failure.");
                return;
            }
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
        
        private void Update()
        {
            m_GameState?.OnUpdate(TimeUtils.DeltaTime());
            
            //@TEMP:
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
        
        private void UnRegisterListener()
        {
            //游戏关闭时事件系统自动清理
            //GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, EntityExtension.OnShowEntitySuccess);
            //GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, EntityExtension.OnShowEntityFail);
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
        
        private void OnLowMemory()
        {
            Debug.LogWarning("[GameCore]OnLowMemory: Memory is low!");
        }
        
        /// <summary>
        /// 如果应用程序暂停时,则为True,否则为False
        /// 用在手机端，按Home键时
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Debug.Log("[Test] 暂停");
            }
            else
            {
                Debug.Log("[Test] 运行");
            }
        }
 
        /// <summary>
        /// 如果应用程序具有焦点时，为True,否则为False
        /// 用在Windows上
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus(bool focus)
        {
            if (focus)  //当程序进入前台时
            {
                Debug.Log("[Test] 前台");
            }
            else        //当程序进入到后台时
            {
                Debug.Log("[Test] 后台"); 
            }
        }
 
        /// <summary>
        /// 当应用程序退出之前发送给所有的游戏对象
        /// IOS有回调，android没有对调
        /// </summary>
        private void OnApplicationQuit()
        {
            //LocalizationManager.Instance.ShutDown();
            //NetManager.Instance.ShutDown();
            Debug.Log("游戏退出了！！");
        }
    }
}