using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Client
{
    public class BaseMeshLoaderLogicSocket:EntityLogicSocketBase
    {
        public BaseMeshLoader AvatarMeshLoader { get; private set; }

        public BaseMeshLoader.MeshLoadCompleteCallback MeshLoadCompleteCallBack;
        
        //默认的avatar prefab名字和挂载socket
         [SerializeField]
        public Transform DefaultMeshSocketTs;
        [SerializeField]
        private string m_MeshPrefabName = "";
        public string MeshPrefabName
        {
            get => m_MeshPrefabName;
            set
            {
                m_MeshPrefabName = value;
                if(AvatarMeshLoader != null)
                    AvatarMeshLoader.AvatarAssetName = value;
            }
        }
        /// <summary>
        /// 创建meshloader，每个继承的pawn需要自己实现，一般建议挂在avatar上的每一个部件，都创建一个自己的meshloader,并注册给回调OnAvatarMeshLoadComplete
        /// </summary>
        public BaseMeshLoaderLogicSocket()
        {
            
        }

        public override void OnShow()
        {
            base.OnShow();
            
            
        }

        protected override void OnInit()
        {
            base.OnInit();
            //在OnInit中时MeshPrefabName还没有设置
            AvatarMeshLoader = new BaseMeshLoader( Entity, m_MeshPrefabName,DefaultMeshSocketTs,false);
            AvatarMeshLoader.MeshLoadComplete += OnMeshLoadComplete;
        }

        protected override void OnHide(bool isShutdown)
        {
            base.OnHide(isShutdown);
            m_LoadComplete = false;
            AvatarMeshLoader?.ShutDown();
        }

                
        public override void Reset()
        {
            AvatarMeshLoader?.Reset();
        }

        public virtual void BeginLoadMesh()
        {
            AvatarMeshLoader?.BeginLoadMesh();
        }

        public virtual void SetMeshAttachedToSocket(Transform transform,Action<object> callback,Vector3 pos = default,Vector3 rot = default)
        {
            AvatarMeshLoader?.SetMeshAttachedToSocket(transform,callback,pos,rot);
        }
        
        protected virtual void OnMeshLoadComplete(BaseMeshLoader meshloader)
        {
            //通知其他proxy,角色模型加载成功
            m_LoadComplete = true;
            MeshLoadCompleteCallBack?.Invoke(meshloader);
        }

        protected bool m_LoadComplete = false;
        public virtual bool LoadComplete
        {
            get
            {
                return m_LoadComplete;
            }
        }

        /// <summary>
        /// 查找mesh上某一个transform socket
        /// </summary>
        /// <param name="socketName"></param>
        /// <returns></returns>
        public virtual Transform FindTransformFromAvatarMesh(string socketName)
        {
            return AvatarMeshLoader?.FindTransformFromAvatarMesh(socketName);
        }

    }
}