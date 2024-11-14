using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Core
{
    public interface IPoolable
    {
        void OnSpawn();

        void OnDespawn();
    }

    public class DelayDeSpawnItem
    {
        public float T;
        public float t;
        public GameObject obj;
        public Action WhenDeSpawn;

        public void Reset()
        {
            T = 0;
            t = 0;
            obj = null;
            WhenDeSpawn = null;
        }
    }

    public class GameObjectCreatorWrapper : IDisposable
    {
        private string assetPath;
        //每次调用时候传进来
        public bool async;
        //同一个资源的多次实例化，会有多个不同的callback，按照队列依次回调一次
        private Queue<Action<GameObject>> asyncCallbackQueue = new Queue<Action<GameObject>>();
        
        //从GameObjectPool里面传进来
        private Dictionary<GameObject, string> Item2Prefab;
        private Func<string, GameObject> DoInstantiate { get; set; }
        private Action<string, Action<GameObject>,Transform> AsyncDoInstantiate { get; set; }
        
        public Action<GameObject> DoDestroy { get; set; }
        private InternalGameObjectPool pool;

        public int TotalCount => pool.TotalCount;
        public string AssetPath => assetPath;
        
        public GameObjectCreatorWrapper(GameObjectPool gameObjectPool,string _assetPath,Dictionary<GameObject, string> _Item2Prefab, Action<GameObject> _DoDestroy,
            Func<string, GameObject> _DoInstantiate, Action<string, Action<GameObject>, Transform> _AsyncDoInstantiate)
        {
            assetPath = _assetPath;
            Item2Prefab = _Item2Prefab;
            DoInstantiate = _DoInstantiate;
            AsyncDoInstantiate = _AsyncDoInstantiate;
            DoDestroy = _DoDestroy;
             pool = new InternalGameObjectPool(
                    $"{assetPath}",
                    gameObjectPool,
                    /*() =>
                    {
                        GameObject item = null;
                        if (async)
                        {
                            AsyncDoInstantiate?.Invoke(assetPath, (go) =>
                            {
                                item = go;
                                if (asyncCallback != null)
                                {
                                    asyncCallback(item);
                                }
                            },null);
                        }
                        else
                        {
                            item = DoInstantiate?.Invoke(assetPath);
                        }
                        if (item != null)
                        {
                            item.GetComponent<IPoolable>()?.OnSpawn();
                            Item2Prefab.Add(item, assetPath);
                        }
                        else if(!async)
                        {
                            Debug.LogError($"No asset {assetPath}");
                        }

                        return item;
                    },*/
                    CreateNewObject,
                    item =>
                    {
                        if (item != null)
                        {
                            item.SetActive(true);
                            item.GetComponent<IPoolable>()?.OnSpawn();
                            
                        }
                    },
                    item =>
                    {
                        if (item != null)
                        {
                            item.GetComponent<IPoolable>()?.OnDespawn();
                            item.SetActive(false);
                            item.transform.SetParent(pool.Root);
                        }
                    },
                    item => { DoDestroy?.Invoke(item); });
        }

        public GameObject Get(bool _async = false,Action<GameObject> _asyncCallback = null)
        {
            async = _async;
            if (async && _asyncCallback != null)
            {
                asyncCallbackQueue.Enqueue(_asyncCallback);
            }
            GameObject gameObject = pool.Get();
            if (async && gameObject != null)
            {
                //如果是异步创建，并且当前帧就获取到了这个资源，要立刻调用这个回调
                if (asyncCallbackQueue.Count > 0)
                {
                    var asyncCallback = asyncCallbackQueue.Dequeue();
                    if (asyncCallback != null)
                    {
                        asyncCallback(gameObject);
                    }
                }
            }

            return gameObject;
        }

        public void Release(GameObject item)
        {
            pool.Release(item);
        }

        public void Clear()
        {
            pool.Clear();
        }
        
        
        public GameObject CreateNewObject()
        {
            GameObject item = null;
            if (async)
            {
                AsyncDoInstantiate?.Invoke(assetPath, (go) =>
                {
                    item = go;
                    item.transform.SetParent(pool.Root);
                    if (asyncCallbackQueue.Count > 0)
                    {
                        var asyncCallback = asyncCallbackQueue.Dequeue();
                        if (asyncCallback != null)
                        {
                            asyncCallback(item);
                        }
                    }
                    //创建出来一个新的，但是在之前Get()时候并没有真正的创建，所以在InternalGameObjectPool的InUsed里面是不包含的，这里要加下
                    if (item != null)
                    {
                        item.GetComponent<IPoolable>()?.OnSpawn();
                        if (!Item2Prefab.ContainsKey(item))
                        {
                            Item2Prefab.Add(item, assetPath);
                        }
                        
                    }

                    pool.AddInUsedAssets(item);
                },null);
            }
            else
            {
                item = DoInstantiate?.Invoke(assetPath);
                if(item)
                    item.transform.SetParent(pool.Root);
            }
            if (item != null)
            {
                item.GetComponent<IPoolable>()?.OnSpawn();
                if (!Item2Prefab.ContainsKey(item))
                {
                    Item2Prefab.Add(item, assetPath);
                }
            }
            else if(!async)
            {
                Debug.LogError($"No asset {assetPath}");
            }

            return item;
        }
        

        
        private class InternalGameObjectPool : IDisposable
        {
            public Transform Root { get; }
            public int TotalCount => ObjectPool.CountInactive + InUsed.Count;
            public Action<GameObject> DoDestroy { get; set; }
            private readonly HashSet<GameObject> InUsed = new HashSet<GameObject>();
            private IObjectPool<GameObject> ObjectPool;

            public InternalGameObjectPool(
                string prefabName,
                GameObjectPool gameObjectPool,
                Func<GameObject> createFunc,
                Action<GameObject> actionOnGet = null,
                Action<GameObject> actionOnRelease = null,
                Action<GameObject> actionOnDestroy = null)
            {
                ObjectPool = new UnityEngine.Pool.ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy);
                Root = new GameObject().transform;

                if (gameObjectPool.Root != null && 
                    gameObjectPool.IsDestroyed == false)    //对于gameObjectPool销毁的同帧进行的操作不能通过==null判断
                {
                    Root.transform.SetParent(gameObjectPool.Root.transform);
                }
                    
                if (Application.isEditor)
                {
                    Root.name = $"[{prefabName.Split('/')[^1]}]";
                    foreach (Transform item in GetTransforms(gameObjectPool.gameObject))
                    {
                        item.SetAsLastSibling();
                    }
                }
            }

            public void AddInUsedAssets(GameObject go)
            {
                if (InUsed.Contains(go) == false)
                {
                    InUsed.Add(go);
                }
            }

            public GameObject Get()
            {
                var o = ObjectPool.Get();
                if (InUsed.Contains(o) == false)
                {
                    InUsed.Add(o);
                }
                return o;
            }
            
            public void Release(GameObject item)
            {
                if (InUsed.Remove(item))
                {
                    ObjectPool.Release(item);
                }
            }

            public void Clear()
            {
                foreach (var item in InUsed)
                {
                    ObjectPool.Release(item);
                    DoDestroy?.Invoke(item);
                }

                InUsed.Clear();
                ObjectPool.Clear();
                if (Root!= null)
                {
                    GameObject.Destroy(Root.gameObject);
                }
                //Destroy(Root.gameObject);
            }

            private Transform[] GetTransforms(GameObject parentGameObject)
            {
                if (parentGameObject != null)
                {
                    var components = new List<Component>(parentGameObject.GetComponentsInChildren(typeof(Transform)));
                    var transforms = components.ConvertAll(c => (Transform)c);

                    transforms.Remove(parentGameObject.transform);
                    transforms.Sort((a, b) => a.name.CompareTo(b.name));

                    return transforms.ToArray();
                }

                return null;
            }

            public void Dispose()
            {
                Clear();
                
                if (ObjectPool is UnityEngine.Pool.ObjectPool<GameObject> pool)
                {
                    pool.Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (pool != null)
            {
                pool.Dispose();
            }
        }
    }

    public class GameObjectPool : MonoBehaviour
    {
        public GameObject Root => gameObject;
        //private Dictionary<string, InternalGameObjectPool> Pools = new();
        private Dictionary<GameObject, string> Item2Prefab = new();
        private List<DelayDeSpawnItem> _delayDeSpawnItems = new();

        private IObjectPool<DelayDeSpawnItem> _delayDeSpawnItemPool = new UnityEngine.Pool.ObjectPool<DelayDeSpawnItem>(
            () => new DelayDeSpawnItem(),
            item => { item.Reset(); },
            item => { item.Reset(); });

        public Func<string, GameObject> DoInstantiate { get; set; }
        public Action<string, Action<GameObject>,Transform> AsyncDoInstantiate { get; set; }
        public Action<GameObject> DoDestroy { get; set; }

        //每一个类型的资源，都对应一个自己的GameObjectCreatorWrapper,同时对应自己的一个asyncCallbackQueue
        private Dictionary<string, GameObjectCreatorWrapper> _gameObjectCreatorWrappers = new Dictionary<string, GameObjectCreatorWrapper>();

        public Dictionary<string, GameObjectCreatorWrapper> GameObjectCreatorWrappers => _gameObjectCreatorWrappers;
        
        public bool IsDestroyed { get; private set; }

        public GameObject Spawn(string assetPath,bool async = false,Action<GameObject> asyncCallback = null)
        {
            var key = assetPath;
            /*if (Pools.TryGetValue(key, out var pool))
            {
                return pool.Get();
            }*/
            if (_gameObjectCreatorWrappers.TryGetValue(key, out var gameObjectCreatorWrapper))
            {
                var spawn = gameObjectCreatorWrapper.Get(async,asyncCallback);
                if (spawn == null && !async)
                {
                    Log.Error($"[MeshLoader]:m_AvatarAssetName:{assetPath} Load Error,Check Mapper Please");
                }
                return spawn;
            }
            else
            {
                GameObjectCreatorWrapper wrapper = new GameObjectCreatorWrapper(this,assetPath,Item2Prefab,DoDestroy,DoInstantiate,AsyncDoInstantiate);
                _gameObjectCreatorWrappers.Add(assetPath,wrapper);
                
                /*pool = new InternalGameObjectPool(
                    $"{assetPath}",
                    this,
                    () =>
                    {
                        GameObject item = null;
                        if (async)
                        {
                            AsyncDoInstantiate?.Invoke(assetPath, (go) =>
                            {
                                item = go;
                                if (asyncCallback != null)
                                {
                                    asyncCallback(item);
                                }
                            },null);
                        }
                        else
                        {
                            item = DoInstantiate?.Invoke(assetPath);
                        }
                        if (item != null)
                        {
                            item.GetComponent<IPoolable>()?.OnSpawn();
                            Item2Prefab.Add(item, key);
                        }
                        else if(!async)
                        {
                            Debug.LogError($"No asset {assetPath}");
                        }

                        return item;
                    },
                    item =>
                    {
                        if (item != null)
                        {
                            item.SetActive(true);
                            item.GetComponent<IPoolable>()?.OnSpawn();
                            item.transform.SetParent(pool.Root);
                        }
                    },
                    item =>
                    {
                        if (item != null)
                        {
                            item.GetComponent<IPoolable>()?.OnDespawn();
                            item.SetActive(false);
                            item.transform.SetParent(pool.Root);
                        }
                    },
                    item => { DoDestroy?.Invoke(item); });*/
                //Pools.Add(key, pool);
            }

            return Spawn(assetPath,async,asyncCallback);
        }

       
        public void DeSpawn(GameObject item)
        {
            if (item == null)
            {
                return;
            }

            if (Item2Prefab.TryGetValue(item, out var key) == false)
            {
                DoDestroy?.Invoke(item);
            }

            if (!string.IsNullOrEmpty(key))
            {
                /*if (Pools.TryGetValue(key, out var pool) == false)
                {
                    DoDestroy?.Invoke(item);
                }
                //TODO 检查节点下的所有managed asset，将其也对应的回pool
                pool.Release(item);*/
                if (_gameObjectCreatorWrappers.TryGetValue(key, out var gameObjectCreatorWrapper))
                {
                    gameObjectCreatorWrapper.Release(item);
                }
                else
                {
                    DoDestroy?.Invoke(item);
                }
            }
            
        }

        public void DeSpawn(GameObject unityGameObject, float delayTime, Action whenDeSpawn = null)
        {
            if (delayTime <= 0)
            {
                whenDeSpawn?.Invoke();
                DeSpawn(unityGameObject);
            }
            else
            {
                var item = _delayDeSpawnItemPool.Get();
                {
                    item.T = delayTime;
                    item.t = 0;
                    item.obj = unityGameObject;
                    item.WhenDeSpawn = whenDeSpawn;
                }
                _delayDeSpawnItems.Add(item);
            }
        }

        public void Clear()
        {
            /*foreach (var pool in Pools.Values)
            {
                pool.Clear();
            }

            Pools.Clear();*/
            foreach (var objectCreatorWrapper in _gameObjectCreatorWrappers.Values)
            {
                objectCreatorWrapper.Clear();
            }
            Item2Prefab.Clear();
            _delayDeSpawnItems.Clear();
            _delayDeSpawnItemPool.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = _delayDeSpawnItems.Count - 1; i >= 0; i--)
            {
                var item = _delayDeSpawnItems[i];
                if ((item.t += deltaTime) >= item.T)
                {
                    item.WhenDeSpawn?.Invoke();
                    DeSpawn(item.obj);
                    _delayDeSpawnItems.RemoveAt(i);
                    _delayDeSpawnItemPool.Release(item);
                }
            }
        }
        
        private void OnDestroy()
        {
            foreach (var objectCreatorWrapper in _gameObjectCreatorWrappers.Values)
            {
                objectCreatorWrapper.Dispose();
            }
            Item2Prefab.Clear();
            _delayDeSpawnItems.Clear();
            _delayDeSpawnItemPool.Clear();
            IsDestroyed = true;     // 关闭编辑器造成的GO销毁并不会立刻让GO==null,给个标记方便其他地方判断
        }

    }
}