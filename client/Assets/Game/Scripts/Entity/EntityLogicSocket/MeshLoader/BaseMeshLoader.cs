
using UnityEngine;
using System;
using System.Collections.Generic;
using Game.Gameplay;
using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.Client
{
     
    public enum MeshLoadingState
    {
        Init = 0,
        AvatarModelLoading,
        AvatarModelLoaded,
        AvatarModelAttachToSocket,          //装备到指定的节点
        AvatarModelHasAttachedToSocket,     //已经装备到了某个指定的节点
        AvatarModelAttachToDefaultSocket,   //恢复原始状态，装备到默认的节点
        AvatarModelDestroying,              //销毁模型
    }

    public class MeshInstanceBase : ObjectBase
    {
        public static MeshInstanceBase Create(string assetName, GameObject go)
        {
            var meshInstanceBase = ReferencePool.Acquire<MeshInstanceBase>();
            meshInstanceBase.Initialize(assetName, go);
            return meshInstanceBase;

        }
        protected override void Release(bool isShutdown)
        {
            GameObject go = (GameObject)Target;
            if (go == null)
                return;
            
            Object.Destroy(go);
        }
    }
    public class BaseMeshLoader
    {
        public delegate void MeshLoadCompleteCallback(BaseMeshLoader meshloader);
        private LoadAssetCallbacks m_LoadAssetCallbacks;

        protected Entity m_AttachedEntity;
        protected MeshLoadingState m_CurrentAvatarState = MeshLoadingState.Init;
        protected string m_AvatarAssetName;

        private static IObjectPool<MeshInstanceBase> m_MeshPool;
        
        //for find some tranform from loaded mesh
        protected SocketFindFactory transformSocketFinder;

        public string AvatarAssetName
        {
            set
            {
                m_AvatarAssetName =  AssetUtility.GetPrefabAsset(value);
            }
        }
        
        protected Transform m_DefaultAvatarSocketTS;           //加载的模型挂在的父节点
        
        public MeshLoadCompleteCallback MeshLoadComplete;
        protected GameObject m_AvatarMesh;

        public GameObject AvatarMesh
        {
            get
            {
                return m_AvatarMesh;
            }
        }

        private Renderer m_Renderer;
        private List<Renderer> m_AllRenderer = new List<Renderer>();
        private List<Material> m_AllMaterials = new List<Material>();


        
        public Renderer MeshRenderer
        {
            get
            {
                if (AvatarMesh == null)
                    return null;
                
                if(m_Renderer == null)
                    m_Renderer = AvatarMesh.GetComponentInChildren<Renderer>();
                
                return m_Renderer;
            }
            set => m_Renderer = value;
        }

        public List<Renderer> AllRenderer
        {
            get
            {
                if (m_AllRenderer.Count <= 0)
                {
                    m_AllRenderer.AddRange(AvatarMesh.GetComponentsInChildren<Renderer>());
                }

                return m_AllRenderer;
            }
        }
        

        public Material SharedMaterial
        {
            get
            {
                if (MeshRenderer == null)
                    return null;
                
                return MeshRenderer.sharedMaterial;
            }
        }

        public List<Material> AllMaterials
        {
            get
            {
                // if (MeshRenderer == null)
                //     return null;
                // return MeshRenderer.materials;
                if (m_AllMaterials.Count <= 0)
                {
                    foreach (var renderer in AllRenderer)
                    {
                        m_AllMaterials.AddRange(renderer.materials);
                    }
                }

                return m_AllMaterials;
            }
        }
        
        public Material[] AllSharedMaterials
        {
            get
            {
                if (MeshRenderer == null)
                    return null;
                return MeshRenderer.sharedMaterials;
            }
        }
        
        private bool _asyncLoad = false;
        public BaseMeshLoader(Entity entity,string roleAvatarAsset,Transform parentTs,bool asyncLoad)
        {
            // if (string.IsNullOrEmpty(roleAvatarAsset))
            // {
            //     Log.Warning("Load Mesh asset name is null or empty, check");
            //     return;
            // }

            //初始化
            if (m_MeshPool == null)
            {
                m_MeshPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<MeshInstanceBase>("MeshBase"
                    , 30.0f, 10, 30.0f
                    , Constant.AssetPriority.GameplayAsset);
            }
            
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadMeshAssetSuccess, OnLoadMeshAssetFail);
            
            m_AttachedEntity = entity;
            AvatarAssetName = roleAvatarAsset;
            
            m_DefaultAvatarSocketTS = parentTs;
            transformSocketFinder = new SocketFindFactory();
            _asyncLoad = asyncLoad;
        }

        //更换模型，可能要reset
        public virtual void Reset()
        {
            m_AvatarMesh = null;
            m_AllRenderer.Clear();
            m_AllMaterials.Clear();
        }

        /// <summary>
        /// 初始化主动加载角色avatar
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void BeginLoadMesh()
        {
            TransitionToState(MeshLoadingState.AvatarModelLoading);
        }
        
        /// <summary>
        /// 主动销毁模型mesh
        /// </summary>
        public virtual void ShutDown()
        {
            TransitionToState(MeshLoadingState.AvatarModelDestroying);
            transformSocketFinder = null;
            m_AllRenderer.Clear();
            m_AllMaterials.Clear();
        }
        
        
        private void TransitionToState(MeshLoadingState newState)
        {
            MeshLoadingState tmpInitialState = m_CurrentAvatarState;
            m_CurrentAvatarState = newState;  
            OnStateEnter(newState, tmpInitialState);
        }

        
        private void OnStateEnter(MeshLoadingState state, MeshLoadingState fromState)
        {
            switch (state)
            {
                case MeshLoadingState.Init:
                {
                    break;
                }
                case MeshLoadingState.AvatarModelLoading:
                    {
                        //开始加载avatar
                        LoadAvatarModel();
                        break;
                    }
                case MeshLoadingState.AvatarModelLoaded:
                    {
                        //加载avatar完成
                        if (DelayAttachedToTransform != null)
                        {
                            TransitionToState(MeshLoadingState.AvatarModelAttachToSocket);
                        }
                        break;
                    }
                case MeshLoadingState.AvatarModelAttachToSocket:
                {
                    AttachedMeshToTargetSocket();
                    break;
                }
                case MeshLoadingState.AvatarModelAttachToDefaultSocket:
                {
                    ResetMeshToDefaultActorSocket();
                    break;
                }
                case MeshLoadingState.AvatarModelDestroying:
                {
                    DestroyModel();
                    break;
                }
                default:
                    break;
            }
        }
        
        private void OnLoadMeshAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            if (m_CurrentAvatarState != MeshLoadingState.AvatarModelLoading)
            {
                Log.Warning($"Mesh is not loading state, currentState:{m_CurrentAvatarState} but loading completed, check if destroy when loading");
                return;
            }
            
            GameObject avatarMesh = Object.Instantiate((GameObject)asset);
            
            m_MeshPool.Register(MeshInstanceBase.Create(assetName, avatarMesh),true );
            OnAvatarLoadedComplete(m_AvatarAssetName, avatarMesh);
            TransitionToState(MeshLoadingState.AvatarModelLoaded);

        }
        private void OnLoadMeshAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error($"Load resource fail, assetName; {assetName}, status: {status}, errorMsg: {errorMessage}");
        }
        
        
        private void LoadAvatarModel()
        {
            Log.Debug($"[mesh] LoadAvatarModel,id={m_AttachedEntity.Id}, {m_AvatarAssetName}");

            var poolService = m_MeshPool;
            if (poolService == null)
                return;
            if (string.IsNullOrEmpty(m_AvatarAssetName))
                return;
            //根据类型，同步或者异步加载
            
            var objectBase = poolService.Spawn(m_AvatarAssetName);
            if (objectBase != null)
            {
                OnAvatarLoadedComplete(m_AvatarAssetName, (GameObject)objectBase.Target);
                TransitionToState(MeshLoadingState.AvatarModelLoaded);

            }
            else
            {
                GameEntry.Resource.LoadAsset(m_AvatarAssetName, m_LoadAssetCallbacks);
            }
        }
        

        private void DestroyModel()
        {
            var poolService = m_MeshPool;
            if (poolService == null)
            {
                if(m_AvatarMesh != null)
                    GameObject.Destroy(m_AvatarMesh);
                return;
            }

            if (m_AvatarMesh != null)
            {
                poolService.Unspawn(m_AvatarMesh);
                m_AvatarMesh = null;
            }
        }
        
        protected virtual void OnAvatarLoadedComplete(string assetName, GameObject obj)
        {
            Log.Debug($"[mesh] OnAvatarLoadedComplete,id={m_AttachedEntity.Id}, {m_AvatarAssetName}");
            if (m_AttachedEntity == null)
            {
                Log.Error($"[MESH_LOG]:AttachedEntity = null,Avatar Loaded Error: assetName:{assetName}");
                return;
            }
            if (obj == null)
            {
                Log.Error($"[MESH_LOG]:{m_AttachedEntity.Id},Avatar Loaded Error: assetName:{assetName}");
                return;
            }

#if !SHIPPING_EXTERNAL
            Log.Debug($"[MESH_LOG]:{m_AttachedEntity.Id},PawnAvatar load complete,asset name is {assetName},pawn shell obj name is {obj.name}");
#endif
            if (m_DefaultAvatarSocketTS != null)
            {
                obj.transform.parent = m_DefaultAvatarSocketTS;
            }
            
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            if (m_AvatarMesh != null)
            {
                Log.Error("[Jy]:Load model complete,but old avatar mesh is not null");
            }

            m_AvatarMesh = obj;
            
            obj.SetActive(true);
            
            TransformSocketMap PawmMeshSocketMap = obj.GetComponent<TransformSocketMap>();
#if UNITY_EDITOR
            if (PawmMeshSocketMap == null)
            {
                Log.Warning($"[BaseMeshLoader],load mesh {m_AvatarMesh.name} complete ,but not find TransformSocketMap,maybe it is not necessary for this prefab");
            }
#endif
            if (transformSocketFinder != null)
            {
                transformSocketFinder.SetTransformSocketMap(PawmMeshSocketMap); //初始化socket map
            }
            else
            {
                Log.Error($"[MESH_LOG]:{m_AttachedEntity.Id},transformSocketFinder = null");
            }
            Log.Debug($"[mesh] OnAvatarLoadedComplete, after SetTransformSocketMap, {PawmMeshSocketMap!=null}, id={m_AttachedEntity.Id}, {assetName}");
            
            MeshLoadComplete?.Invoke(this); //比如武器挂载到角色socket
        }

        protected virtual void AttachedMeshToTargetSocket()
        {
            m_AvatarMesh.transform.parent = DelayAttachedToTransform;
            m_AvatarMesh.transform.localPosition = AttachedToTransformPosition;
            m_AvatarMesh.transform.localRotation = Quaternion.Euler(AttachedToTransformRotation);
            m_AvatarMesh.transform.localScale = Vector3.one;
            m_AttachedCallBack(m_AvatarMesh);
            TransitionToState(MeshLoadingState.AvatarModelHasAttachedToSocket);
        }
        
        protected virtual void ResetMeshToDefaultActorSocket()
        {
            DelayAttachedToTransform = null;
            if (m_AvatarMesh != null)
            {
                m_AvatarMesh.transform.parent = m_DefaultAvatarSocketTS;
                m_AvatarMesh.transform.localPosition = Vector3.zero;
                m_AvatarMesh.transform.localRotation = Quaternion.identity;
                m_AvatarMesh.transform.localScale = Vector3.one;
            }
            
            TransitionToState(MeshLoadingState.Init);
        }

        private Transform DelayAttachedToTransform = null;
        private Vector3 AttachedToTransformPosition = Vector3.zero;
        private Vector3 AttachedToTransformRotation = Vector3.zero;
        /// <summary>
        /// 在装备mesh到指定socket成功后的回调，可以把mesh GameObject作为参数回调回去，或者重载后自定义
        /// </summary>
        private Action<object> m_AttachedCallBack = null;
        public virtual void SetMeshAttachedToSocket(Transform transform,Action<object> callback,Vector3 pos = default,Vector3 rot = default)
        {
            if (transform == null)
            {
                DelayAttachedToTransform = null;
                m_AttachedCallBack = null;
                if (m_CurrentAvatarState == MeshLoadingState.AvatarModelLoading)
                {
                    //正在loading，延迟   
                }
                else
                {
                    TransitionToState(MeshLoadingState.AvatarModelAttachToDefaultSocket);
                }
            }
            else
            {
                DelayAttachedToTransform = transform;
                AttachedToTransformPosition = pos;
                AttachedToTransformRotation = rot;
                m_AttachedCallBack = callback;
                if (m_CurrentAvatarState == MeshLoadingState.AvatarModelLoading)
                {
                    //正在loading，延迟   
                }
                else
                {
                    TransitionToState(MeshLoadingState.AvatarModelAttachToSocket);
                }
            }
        }

        public Transform FindTransformFromAvatarMesh(string socketName)
        {
            if (transformSocketFinder != null)
            {
                return transformSocketFinder.FindSocket(socketName);
            }
            else
            {
                Log.Warning($"[FindTransformFromAvatarMesh] from {m_AvatarMesh},may be add SocketFindFactory to it is more efficient");
                return SocketUtils.FindSocketTsFromTarget(socketName, m_AvatarMesh.transform, false);
            }
        }

    }
}