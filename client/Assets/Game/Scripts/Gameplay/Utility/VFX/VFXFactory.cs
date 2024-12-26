using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using YooAsset;
using Log = UnityGameFramework.Runtime.Log;


namespace Game.Client
{
    public class BaseVFXLoadInfo: IReference
    {
        public VFXBaseSpawnParam SpawnParam;
        public DRVFX VFXBean;
        public int SerialId;
        public void Clear()
        {
            SpawnParam = null;
            VFXBean = null;
            SerialId = 0;
        }
    }
    public class BaseVFXFactory : IVFXFactory
    {
        protected Dictionary<int, VFXBase> AllVFX = new Dictionary<int, VFXBase>();
        private int _maxVfxCount = -1;
        protected bool NeedRemoveEntityActorWhenDeSpawn;
        private GameFramework.ObjectPool.IObjectPool<VFXInstantBase> m_VFXObjectPool = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks;
        private readonly HashSet<int> m_VfxBeingLoaded;
        private readonly HashSet<int> m_VfxToReleaseOnLoad;
        private static int s_VfxSerialId = 0;

        public static int GenerateVfxSerialId()
        {
            return --s_VfxSerialId;
        }
        public BaseVFXFactory(int maxVfxCount = -1)
        {
            _maxVfxCount = maxVfxCount;
            NeedRemoveEntityActorWhenDeSpawn = false;
            m_VFXObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<VFXInstantBase>("VFXBase", 10);
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadVFXAssetSuccess, OnLoadVFXAssetFail);
            m_VfxBeingLoaded = new HashSet<int>();
            m_VfxToReleaseOnLoad = new HashSet<int>();
        }

        private void OnLoadYooVFXAssetSuccess(GameObject gameObject, string assetName, float duration, object userData)
        {
            if (gameObject == null)
            {
                OnLoadVFXAssetFail(assetName, LoadResourceStatus.AssetError, "load asset is null", userData);
                return;
            }
            var loadInfo = (BaseVFXLoadInfo)userData;

            if (m_VfxToReleaseOnLoad.Contains(loadInfo.SerialId))
            {
                m_VfxToReleaseOnLoad.Remove(loadInfo.SerialId);
                ReferencePool.Release(loadInfo.SpawnParam);
                ReferencePool.Release(loadInfo);
                return;

            }

            var vfxGO = gameObject;//(GameObject)Object.Instantiate((UnityEngine.Object)asset);

            m_VfxBeingLoaded.Remove(loadInfo.SerialId);
            var vfxBase = vfxGO.GetOrAddComponent<VFXBase>();
            vfxBase.SerialId = loadInfo.SerialId;
            m_VFXObjectPool.Register(VFXInstantBase.Create(assetName, vfxBase), true);
            AllVFX.Add(vfxBase.SerialId, vfxBase);
            vfxBase.InitVFXParam(loadInfo.VFXBean, this, loadInfo.SpawnParam);
            //vfxGO.transform.SetParent(loadInfo.GO.transform);
            ReferencePool.Release(loadInfo.SpawnParam);
            ReferencePool.Release(loadInfo);
        }
        
        private void OnLoadVFXAssetSuccess( string assetName, object asset, float duration, object userData)
        {
            if (asset == null)
            {
                OnLoadVFXAssetFail(assetName, LoadResourceStatus.AssetError, "load asset is null", userData);
                return;
            }
            var loadInfo = (BaseVFXLoadInfo)userData;

            if (m_VfxToReleaseOnLoad.Contains(loadInfo.SerialId))
            {
                m_VfxToReleaseOnLoad.Remove(loadInfo.SerialId);
                ReferencePool.Release(loadInfo.SpawnParam);
                ReferencePool.Release(loadInfo);
                return;

            }

            var vfxGO =(GameObject)Object.Instantiate((UnityEngine.Object)asset);

            m_VfxBeingLoaded.Remove(loadInfo.SerialId);
            var vfxBase = vfxGO.GetOrAddComponent<VFXBase>();
            vfxBase.SerialId = loadInfo.SerialId;
            m_VFXObjectPool.Register(VFXInstantBase.Create(assetName, vfxBase), true);
            AllVFX.Add(vfxBase.SerialId, vfxBase);
            vfxBase.InitVFXParam(loadInfo.VFXBean, this, loadInfo.SpawnParam);
            //vfxGO.transform.SetParent(loadInfo.GO.transform);
            ReferencePool.Release(loadInfo.SpawnParam);
            ReferencePool.Release(loadInfo);
        }
        
        private void OnLoadVFXAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error("Load VFX asset fail, check");
        }
        
        public virtual int SpawnVFX(VFXBaseSpawnParam vfxSpawnParam)
        {
            VFXBase vfx = null;
            DRVFX vfxBean = GameEntry.DataTable.GetDataTable<DRVFX>().GetDataRow(vfxSpawnParam.VFXIndexId);

            if (vfxBean == null)
            {
                Log.Error($"[VFX] tbVfxBean == null, vfxSpawnParam.VFXIndexId = {vfxSpawnParam.VFXIndexId} not exist");
                return 0;
            }
            var assetName = AssetUtility.GetVFXAsset(vfxBean.AssetName);
            int vfxSerialId = GenerateVfxSerialId();

            var poolService = m_VFXObjectPool;
            if (poolService != null && vfxBean != null && !string.IsNullOrEmpty(assetName))
            {
                // base类型的vfx factory不做数量限制
                // CheckExceedMaxCount();
                // @todo
                // 根据类型，同步或者异步加载
                VFXInstantBase vfxInstance = poolService.Spawn(assetName);
                
                if (vfxInstance != null)
                {
                    vfx = (VFXBase)vfxInstance.Target;
                    
                    if (vfx == null)
                    {
#if UNITY_EDITOR
                        Log.Error($"[VFX Factory] could not find VFXBase from {assetName},did you forget it ! please check");
#endif
                    }

                    vfx.SerialId = vfxSerialId;
                    //@TODO: 特效挂载
                    // 挂到父物体下
                    // if (vfxSpawnParam.Parent != null) 
                    //     vfx.transform.SetParent(vfxSpawnParam.Parent);
                    
                    vfx.InitVFXParam(vfxBean,this,vfxSpawnParam);
                    AllVFX.Add(vfx.SerialId, vfx);
                    
                    
                    ReferencePool.Release(vfxSpawnParam);
                }
                else
                {
                    //Log.Warning($"[VFX Factory] spawn asset {assetName} fail, create new one");

                    m_VfxBeingLoaded.Add(vfxSerialId);
                    var loadInfo = ReferencePool.Acquire<BaseVFXLoadInfo>();
                    loadInfo.SpawnParam = vfxSpawnParam;
                    loadInfo.VFXBean = vfxBean;
                    loadInfo.SerialId = vfxSerialId;

                    // GameEntry.YooResource.LoadGameObjectAsync(assetName,go =>
                    // {
                    //     OnLoadYooVFXAssetSuccess(go, assetName, 0, loadInfo);
                    // });
                    
                    //GameEntry.Resource.LoadAsset(assetName, m_LoadAssetCallbacks, loadInfo);

                    // //@TODO: 挂载
                    // //vfxGO.transform.SetParent();

                }
            }
            else
            {
                
                
                ReferencePool.Release(vfxSpawnParam);
                Log.Warning($"Spawn vfx {vfxSpawnParam.VFXTypeId} failed ");
            }
            // will auto recycle
            return vfxSerialId;
        }

        // protected virtual void PostSpawnVFX(World world, Entity entity, EntityActor entityActor)
        // {
        // }

        public virtual void DeSpawnVFX(int vfxSerialId)
        {
            if (vfxSerialId != 0)
            {
                var poolService = m_VFXObjectPool;
                if (poolService != null)
                {
                    if (AllVFX.ContainsKey(s_VfxSerialId))
                    {
                        var vfxBase = AllVFX[vfxSerialId];
                        RemoveEntityActorFormManager(vfxBase);
                    
                        poolService.Unspawn(vfxBase);
                        AllVFX.Remove(vfxBase.SerialId);
                    }
                    else
                    {
                        m_VfxToReleaseOnLoad.Add(vfxSerialId);
                    }
                    
                }
            }
        }
        
        protected void RemoveEntityActorFormManager( VFXBase vfx)
        {
            if (!NeedRemoveEntityActorWhenDeSpawn)
                return;
            
            // form AA, not good enough
            // var actor = vfx.gameObject.GetComponent<ParticleEntityActor>();
            // if (actor != null)
            // {
            //     ParticleEntityActorManager.Instance.RemoveActor(actor); //将actor从Manager移除
            // }
        }
        
        public virtual void Update(float deltaime)
        {
            List<VFXBase> removalVfx = ListPool<VFXBase>.Get();
            foreach (var vfx in AllVFX.Values)
            {
                vfx.UpdateVFX(deltaime);
                if (!vfx.VFXIsActive)
                {
                    removalVfx.Add(vfx);
                }
            }

            foreach (var vfx in removalVfx)
            {
                //deSpawn vfx,and remove from list
                RemoveSpecifyIndexVFX(vfx.SerialId);
            }
            
            removalVfx.Clear();
            ListPool<VFXBase>.Release(removalVfx);
        }

        // protected void CheckExceedMaxCount()
        // {
        //     if (AllVFX.Count >= _maxVfxCount)
        //     {
        //         //移除最前面那个
        //         RemoveSpecifyIndexVFX(AllVFX[0].SerialId);
        //     }
        // }

        private void RemoveSpecifyIndexVFX(int serialId)
        {
            VFXBase vfxBase = AllVFX[serialId];
            var poolService = m_VFXObjectPool;
            if (poolService != null)
            {
                if (vfxBase != null)
                {
                    poolService.Unspawn(((VFXBase)vfxBase));
                }
                AllVFX.Remove(serialId);
            }
        }

        public void ShutDown()
        {

            //pool 会自动释放
            // foreach (var vfxBase in AllVFX.Values)
            // {
            //     RemoveSpecifyIndexVFX(vfxBase.SerialId);
            // }
            
            AllVFX.Clear();
            m_VfxBeingLoaded.Clear();
            m_VfxToReleaseOnLoad.Clear();
        }
    }

}