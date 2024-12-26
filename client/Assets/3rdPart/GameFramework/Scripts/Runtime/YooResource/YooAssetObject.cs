using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using YooAsset;

namespace Game.Client
{
    /// <summary>
    /// 资源对象。
    /// </summary>
    public sealed class YooAssetObject : ObjectBase
    {
        private AssetHandle m_AssetHandle;
        private YooResourceComponent m_ResourceManager;

        public AssetHandle Handle => m_AssetHandle;

        public YooAssetObject()
        {
            m_AssetHandle = null;
        }

        public static YooAssetObject Create(string name, object target, object assetHandle, YooResourceComponent yooResourceComponent)
        {
            if (assetHandle == null)
            {
                throw new GameFrameworkException("Resource is invalid.");
            }

            if (yooResourceComponent == null)
            {
                throw new GameFrameworkException("Resource Manager is invalid.");
            }

            YooAssetObject assetObject = ReferencePool.Acquire<YooAssetObject>();
            assetObject.Initialize(name, target);
            assetObject.m_AssetHandle = (AssetHandle)assetHandle;
            assetObject.m_ResourceManager = yooResourceComponent;
            return assetObject;
        }

        public override void Clear()
        {
            base.Clear();
            m_AssetHandle = null;
        }


        protected override void Release(bool isShutdown)
        {
            if (!isShutdown)
            {
                AssetHandle handle = m_AssetHandle;
                if (handle is { IsValid: true })
                {
                    handle.Dispose();
                }
                handle = null;
            }
        }
    }
}