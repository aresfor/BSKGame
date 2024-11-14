using Game.Core;
//using CustomGamePlay;
using UnityEngine;
using System;

namespace Game.Core
{
    public class ObjectPoolUtility : IObjectPoolUtility,IUpdateableUtility
    {
        private bool CleanPoolWhenShutdown = false;
        private Transform Root;
        private static GameObjectPool _gameObjectPool;

        private Func<string, GameObject> m_InstantiateFunc;
        private Action<string, Action<GameObject>,Transform> m_AsyncInsantiateFunc;
        private Action<GameObject> m_DestroyFunc;
        private Func<string, string> m_PathWrapper;
        /// <summary>
        /// 把实际的资源加载和卸载接口 传进来
        /// </summary>
        /// <param name="instantiateFunc"></param>
        /// <param name="destroyFunc"></param>
        public ObjectPoolUtility(
            Func<string, GameObject> instantiateFunc, 
            Action<GameObject> destroyFunc,
            Action<string, Action<GameObject>, Transform> asyncInstantiateFunc,
            Func<string, string> pathWrapper = null)
        {
            m_InstantiateFunc = instantiateFunc == null ? CreateGo : instantiateFunc;
            m_DestroyFunc = destroyFunc == null ? DestroyGo : destroyFunc;
            m_AsyncInsantiateFunc = asyncInstantiateFunc;
            m_PathWrapper = pathWrapper;
        }
        
        public void Initialize()
        {
            if (_gameObjectPool == null)
            {
                _gameObjectPool = new GameObject("InGameObjectPool").AddComponent<GameObjectPool>();
                GameObject.DontDestroyOnLoad(_gameObjectPool);
                _gameObjectPool.DoInstantiate = m_InstantiateFunc;
                _gameObjectPool.AsyncDoInstantiate = m_AsyncInsantiateFunc;
                _gameObjectPool.DoDestroy = m_DestroyFunc;
            }
        }

        public void Shutdown()
        {
            if (CleanPoolWhenShutdown)
            {
                if (_gameObjectPool != null)
                {
                    UnityEngine.Object.DestroyImmediate(_gameObjectPool);
                    _gameObjectPool = null;
                }
            }
        }

        public void Reset()
        {
        }


        public GameObject Spawn(string assetPath)
        {
            if (m_PathWrapper != null)
                assetPath = m_PathWrapper(assetPath);
            return _gameObjectPool?.Spawn(assetPath);
        }

        public void SpawnAsync(string assetPath, Action<GameObject> callback)
        {
            //spawn prefab async
            if (m_PathWrapper != null)
                assetPath = m_PathWrapper(assetPath);
            _gameObjectPool?.Spawn(assetPath, true,callback);
        }

        public void DeSpawn(GameObject unityGameObject)
        {
            _gameObjectPool?.DeSpawn(unityGameObject);
        }

        public void DeSpawn(GameObject unityGameObject, float delayTime)
        {
            _gameObjectPool?.DeSpawn(unityGameObject, delayTime);
        }

        private GameObject CreateGo(string assetPath)
        {
            var asset = Resources.Load<GameObject>(assetPath);
            if (asset != null)
            {
                return GameObject.Instantiate(asset);
            }

            return null;
        }

        private void DestroyGo(GameObject obj)
        {
            if (obj != null)
            {
                GameObject.Destroy(obj);
            }
        }
        
        //处理延时despawn的object
        public void Update(float deltaTime)
        {
            _gameObjectPool?.OnUpdate(deltaTime);
        }
        
    }
}