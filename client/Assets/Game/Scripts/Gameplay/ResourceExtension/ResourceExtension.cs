using System;
using System.Collections;
using System.Collections.Generic;
using Game.Gameplay;
using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.Client
{
    public static class ResourceExtension
    {
        private static IObjectPool<GameObjectInstanceBase> s_GameObjectPool;
        private static LoadAssetCallbacks m_LoadAssetCallbacks;
        private static uint s_GameObjectSerialId = 0;
        private static Dictionary<uint, Action<GameObject, object>> s_LoadCallbackDic;
        private static Dictionary<uint , GameObject> s_GameObjectInstanceDic;
        public static uint GenerateGameObjectSerialId()
        {
            return ++s_GameObjectSerialId;
        }
        public static void Initialize()
        {
            //初始化
            if (s_GameObjectPool == null)
            {
                s_GameObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<GameObjectInstanceBase>("GameObjectBase"
                    , 30.0f, 10, 30.0f
                    , Constant.AssetPriority.GameplayAsset);
                m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFail);
                s_LoadCallbackDic = new Dictionary<uint, Action<GameObject, object>>();
                s_GameObjectInstanceDic = new Dictionary<uint, GameObject>();
            }
        }

        public static uint Instantiate(this YooResourceComponent resourceComponent
            , string assetName
            , Action<GameObject, object> loadSuccessCallback
            , object loadUserData)
        {
            uint serialId = GenerateGameObjectSerialId();

            CoroutineUtils.StartCoroutine(Instantiate(resourceComponent,serialId
                ,assetName,  loadSuccessCallback, loadUserData));
            return serialId;
        }
        
        public static IEnumerator Instantiate(this YooResourceComponent resourceComponent
            , uint serialId
            , string assetName
            , Action<GameObject, object> loadSuccessCallback
            , object loadUserData)
        {
            GameObjectInstanceBase goInstance = s_GameObjectPool.Spawn(assetName);
            if (goInstance != null)
            {
                loadSuccessCallback?.Invoke((GameObject)goInstance.Target, loadUserData);
            }
            else
            {
                VarUInt32 serialIdData = ReferencePool.Acquire<VarUInt32>();
                serialIdData.Value = serialId;
                var info = ReferencePool.Acquire<InstantiateInfo>();
                info.SerialIdData = serialIdData;
                info.LoadUserData = loadUserData;
                
                s_LoadCallbackDic[serialIdData.Value] = loadSuccessCallback; 
                resourceComponent.LoadGameObjectAsync(assetName, (go) =>
                {
                    OnLoadYooAssetSuccess(go, assetName, info);
                });
                //OnLoadYooAssetSuccess(assetName, handle, info);
            }

            return null;
        }
        
        public static void UnInstantiate(this YooResourceComponent resourceComponent, uint serialId)
        {
            if (serialId == 0)
                return;
            
            if (false == s_GameObjectInstanceDic.ContainsKey(serialId))
            {
                Log.Error($"can not find gameobject instance, serialId: {serialId}");
                return;
            }
        
            var go = s_GameObjectInstanceDic[serialId];
            s_GameObjectInstanceDic.Remove(serialId);
            s_GameObjectPool.Unspawn(go);
        }
        
        
        public static void Instantiate(this ResourceComponent resourceComponent
            , uint serialId
            , string assetName
            , Action<GameObject, object> loadSuccessCallback
            , object loadUserData)
        {
            GameObjectInstanceBase goInstance = s_GameObjectPool.Spawn(assetName);
            if (goInstance != null)
            {
                loadSuccessCallback?.Invoke((GameObject)goInstance.Target, loadUserData);
                return;
            }
            else
            {
                VarUInt32 serialIdData = ReferencePool.Acquire<VarUInt32>();
                serialIdData.Value = serialId;
                var info = ReferencePool.Acquire<InstantiateInfo>();
                info.SerialIdData = serialIdData;
                info.LoadUserData = loadUserData;
                
                s_LoadCallbackDic[serialIdData.Value] = loadSuccessCallback; 
                resourceComponent.LoadAsset(assetName, m_LoadAssetCallbacks, info);
            }
            
            
        }

        public static void UnInstantiate(this ResourceComponent resourceComponent, uint serialId)
        {
            if (serialId == 0)
                return;
            
            if (false == s_GameObjectInstanceDic.ContainsKey(serialId))
            {
                Log.Error($"can not find gameobject instance, serialId: {serialId}");
                return;
            }

            var go = s_GameObjectInstanceDic[serialId];
            s_GameObjectInstanceDic.Remove(serialId);
            s_GameObjectPool.Unspawn(go);
        }
        
        private static void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            InstantiateInfo info = (InstantiateInfo)userData;
            VarUInt32 id = info.SerialIdData;
            if (s_LoadCallbackDic.ContainsKey(id.Value))
            {
                GameObject gameObject = Object.Instantiate((GameObject)asset);
                s_LoadCallbackDic[id.Value].Invoke(gameObject, info.LoadUserData);
                s_GameObjectInstanceDic.Add(id.Value, gameObject);
                s_LoadCallbackDic.Remove(id.Value);
                s_GameObjectPool.Register(GameObjectInstanceBase.Create(assetName, id.Value, gameObject),true);
            }
            else
            {
                Log.Error($"can not find gameobject instance accroding to serial id: {id.Value}");
            }
            ReferencePool.Release(info);
        }
        
        private static void OnLoadYooAssetSuccess(GameObject gameObject, string assetName,object userData)
        {
            InstantiateInfo info = (InstantiateInfo)userData;
            VarUInt32 id = info.SerialIdData;
            if (s_LoadCallbackDic.ContainsKey(id.Value))
            {
                //GameObject gameObject = handle.InstantiateSync();
                s_LoadCallbackDic[id.Value].Invoke(gameObject, info.LoadUserData);
                s_GameObjectInstanceDic.Add(id.Value, gameObject);
                s_LoadCallbackDic.Remove(id.Value);
                s_GameObjectPool.Register(GameObjectInstanceBase.Create(assetName, id.Value, gameObject),true);
            }
            else
            {
                Log.Error($"can not find gameobject instance accroding to serial id: {id.Value}");
            }
            ReferencePool.Release(info);
        }
        
        private static void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"Load resource fail, assetName; {assetName}, status: {status}, errorMsg: {errorMessage}");
        }
    }

    public class InstantiateInfo:IReference
    {
        public VarUInt32 SerialIdData;
        public object LoadUserData;
        public void Clear()
        {
            if(SerialIdData != null)
                ReferencePool.Release(SerialIdData);
            LoadUserData = null;
        }
    }
    public class GameObjectInstanceBase : ObjectBase
    {
        public uint SerialId { get; private set; }
        public static GameObjectInstanceBase Create(string assetName, uint serialId, GameObject go)
        {
            var meshInstanceBase = ReferencePool.Acquire<GameObjectInstanceBase>();
            meshInstanceBase.Initialize(assetName, go);
            meshInstanceBase.SerialId = serialId;
            return meshInstanceBase;

        }

        protected override void OnUnspawn()
        {
            GameObject go = (GameObject)Target;
            if (go == null)
                return;
            
            go.SetActive(false);
            
            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            GameObject go = (GameObject)Target;
            if (go == null)
                return;
            
            Object.Destroy(go);
        }
    }
}