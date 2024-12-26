             
using System;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;
using Object = UnityEngine.Object;

namespace Game.Client
{

    /// <summary>
    /// VFX对象池接口类
    /// </summary>
    public class VFXInstantBase : ObjectBase
    {

        public static VFXInstantBase Create(string name, object target)
        {
            VFXInstantBase vfxInstance = ReferencePool.Acquire<VFXInstantBase>();
            vfxInstance.Initialize(name, target);
            return vfxInstance;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            VFXBase vfx = (VFXBase)Target;
            vfx.OnSpawn();


        }

        
        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            VFXBase vfx = (VFXBase)Target;

            vfx.OnDespawn();
        }

        protected override void Release(bool isShutdown)
        {
            VFXBase vfx = (VFXBase)Target;
            
            //game shutdown时候object pool的shutdown可能会比vfx base要迟，因此这里判空也不抛出错误
            if (null == vfx)
            {
                //Log.Error("VFX is null, check type");
                return;
            }
            Object.Destroy(vfx.gameObject);
        }
    }
    
    /// <summary>
    /// 基础VFX，其他类型的特效，根据需要继承
    /// </summary>
    public class VFXBase:BaseMonoBehaviour,IVFX
    {
        [SerializeField]
        private ParticleSystem _particleSystem;
        protected ParticleSystem GetParticleSystem
        {
            get => _particleSystem;
        }
        
        protected IVFXFactory spawnFactory;
        public IVFXFactory SpawnFactory
        {
            get => spawnFactory;
        }

        protected DRVFX VFXBean;
        [Header("生命周期计时")][SerializeField]
        protected float lifeSpanTimer;
        [Header("死亡后的计时")][SerializeField]
        protected bool bStartTimer_AfterDead = false;
        protected int spawnerEntityID;
        protected int receiverEntityID;
        [Header("是否激活")][SerializeField]
        protected bool vfxIsActive;

        //spawn时候设置
        public int SerialId;
        
        public bool VFXIsActive
        {
            get
            {
                return vfxIsActive;
            }
        }

#if UNITY_EDITOR
        public ParticleSystem ParticleSystem
        {
            set
            {
                if (_particleSystem == null)
                    _particleSystem = value;
            }
        }
#endif
        public virtual void InitVFXParam(DRVFX tbvfxBean,IVFXFactory factory,VFXBaseSpawnParam param)
        {
            vfxIsActive = true;
            VFXBean = tbvfxBean;
            lifeSpanTimer = 0;
            spawnFactory = factory;
            InitializeCachedTransform(param);
            spawnerEntityID = param.SpawnerEntityId;
            receiverEntityID = param.ReceiverEntityId;
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            if (_particleSystem != null)
            {
                _particleSystem.Play(true);
            }
            
            // // 特殊判断，是否包含TimeOverWithSpawnerDead
            // for (int i = 0; i < _VFXBeanConfig.DespawnType.Count; ++i)
            // {
            //     switch (_VFXBeanConfig.DespawnType[i])
            //     {
            //         case EDespawnType.TimeOverWithSpawnerDead:
            //         {
            //             bStartTimer_AfterDead = false;
            //             break;
            //         }
            //     }
            // }
            bStartTimer_AfterDead = false;
        }

        /// <summary>
        /// 对于特殊的特效，可能对其Transform有特殊要求，比如LineRenderer
        /// </summary>
        /// <param name="param"></param>
        protected virtual void InitializeCachedTransform(VFXBaseSpawnParam param)
        {
            CachedTransform.position = param.Position.ToVector3();
            CachedTransform.rotation = param.Rotation.ToQuaternion();
            CachedTransform.localScale = param.Scale.ToVector3();
        }

        

        public virtual void ReplayVFX()
        {
            if (_particleSystem != null)
            {
                _particleSystem.Play(true);
            }
        }

        public virtual void PauseVFX()
        {
        }
        

        public void UpdateVFX(float deltaTime)
        {
            switch (VFXBean.DespawnType)
            {
                case 1:
                    ProcessReceiverLifeSpan();
                    break;
                case 2:
                    ProcessSpawnerLifeSpan();
                    break;
                case 3:
                    ProcessTimerLifeSpan(deltaTime);
                    break;
                default:
                    Log.Error("VFX despawn type case miss match, check");
                    break;
            }
            // for (int i = 0; i < VFXBean.DespawnType.Count; ++i)
            // {
            //     switch (VFXBean.DespawnType[i])
            //     {
            //         case EDespawnType.TimeOver:
            //         {
            //             ProcessTimerLifeSpan(deltaTime);
            //             break;
            //         }
            //         case EDespawnType.WithReceiverDead:
            //         {
            //             ProcessReceiverLifeSpan();
            //             break;
            //         };
            //         case EDespawnType.WithSpawnerDead:
            //         {
            //             ProcessSpawnerLifeSpan();
            //             break;
            //         }
            //         case EDespawnType.TimeOverWithSpawnerDead:
            //         {
            //             ProcessTimerLifeSpanWithSpawnerDead(deltaTime);
            //             break;
            //         }
            //         case EDespawnType.GameOver:
            //         {
            //             ProcessGameOverLifeSpan();
            //             break;
            //         }
            //     }
            // }

            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime)
        {
            
        }

        /// <summary>
        /// 处理定时器生命周期
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void ProcessTimerLifeSpan(float deltaTime)
        {
            lifeSpanTimer += deltaTime;
            if (lifeSpanTimer > VFXBean.Lifetime)
            {
                vfxIsActive = false;
            }
        }

        public Transform poolRootParent;
        /// <summary>
        /// 处理特效生成者死亡后定时销毁的生命周期
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void ProcessTimerLifeSpanWithSpawnerDead(float deltaTime)
        {
            // 如果计时还没开始，看下目标死了没
            if (!bStartTimer_AfterDead)
            {
                if (spawnerEntityID != 0)
                {
                    Entity entity = GameEntry.Entity.GetEntity(spawnerEntityID);
                    // 开始延迟销毁计时
                    if (entity == null || entity.HasTag(Constant.GameplayTag.Dead))
                    {
                        // 特殊处理：
                        // e.g.子弹拖尾特效会在生成的时候放到子弹的下面，而子弹本身回收隐藏(SetActive(false))的时候，会连带隐藏下面的子物体，
                        // 因此可以在延迟销毁开始的时候，把特效移到特效池EffectPool的root下面
                        // （问题：现在外部访问不到特效池的root）
                        // this.transform.SetParent(pool.root);
                        // 所以用以下方法代替
                        if (poolRootParent != null)
                            this.transform.SetParent(poolRootParent);
                        bStartTimer_AfterDead = true;
                    }
                }
            }

            if (bStartTimer_AfterDead)
            {
                lifeSpanTimer += deltaTime;
                if (lifeSpanTimer > VFXBean.Lifetime)
                {
                    vfxIsActive = false;
                }
            }
        }

        /// <summary>
        /// 处理接收者销毁的生命周期
        /// </summary>
        protected virtual void ProcessReceiverLifeSpan()
        {
            if (receiverEntityID != 0)
            {
                Entity entity = GameEntry.Entity.GetEntity(receiverEntityID);
                if (entity != null && entity.HasTag(Constant.GameplayTag.Dead))
                {
                    vfxIsActive = false;
                }
            }
        }

        /// <summary>
        /// 处理特效生成者的生命周期
        /// </summary>
        protected virtual void ProcessSpawnerLifeSpan()
        {
            if (spawnerEntityID != 0)
            {
                Entity entity = GameEntry.Entity.GetEntity(spawnerEntityID);
                //如果创建者都没了，也要销毁
                if (entity is null)
                {
                    vfxIsActive = false;
                }
                if (entity != null && entity.HasTag(Constant.GameplayTag.Dead))
                {
                    vfxIsActive = false;
                }
            }
        }

        /// <summary>
        /// 游戏结束时候，处理特效
        /// </summary>
        protected virtual void ProcessGameOverLifeSpan()
        {
            // var matchPhaseBaseComponent = SpawnWorld.GetSingletonComponent<MatchPhaseBaseComponent>();
            // if (matchPhaseBaseComponent.CurrentMatchPhase == EGameMatchPhase.kMatchPhaseGameOver ||
            //     matchPhaseBaseComponent.CurrentMatchPhase == EGameMatchPhase.kMatchPhaseGameShutdown)
            // {
            //     vfxIsActive = false;
            // }

            throw new NotImplementedException();
        }

        public void OnSpawn()
        {
            if(_particleSystem)
                _particleSystem.Play(true);
        }

        public void OnDespawn()
        {
            //先active为false，在despawn
            //vfxIsActive = false;
            if (_particleSystem)
                _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}