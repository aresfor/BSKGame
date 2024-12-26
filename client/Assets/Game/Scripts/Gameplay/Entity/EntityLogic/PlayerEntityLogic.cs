using System;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class PlayerEntityLogic:GameEntityLogic
    {
        private RoleEntityModel m_Model;
        //private IEntityLogic m_LastHitEntityLogic;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (RoleEntityModel)userData;
            var healthBindable = m_Model.GetBindableProperty(EPropertyDefine.Health);
            healthBindable.Register((oldValue, newValue) =>
                Log.Info($"Health Change To: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
            
            m_Model.SetProperty(EPropertyDefine.Health, 110);
            
            AddTag("Entity.Effect.Frozen");
            
            bool exact = HasTag("Entity.Effect.Burn", EGameplayTagCheckType.Exact);
            bool parent = HasTag("Entity.Effect", EGameplayTagCheckType.Parent);
            
            Log.Info($"exact: {exact}, parent: {parent}");
            
            //vfx
            VFXBaseSpawnParam spawnParam = ReferencePool.Acquire<VFXBaseSpawnParam>();
            spawnParam.SpawnerEntityId = Id;
            spawnParam.ReceiverEntityId = Id;
            spawnParam.Position = Vector3.forward.ToFloat3() * 5.0f;
            spawnParam.Scale = Vector3.one.ToFloat3();
            spawnParam.VFXIndexId = 1001;
            spawnParam.VFXTypeId = (int)VFXType.Base;
            var vfxSerialId = VFXUtils.SpawnVFX(spawnParam);

            VFXUtils.DeSpawnVFX(vfxSerialId);

            var meshLoaderSocket = FindLogicSocket<BaseMeshLoaderLogicSocket>();

            //model
            meshLoaderSocket.MeshLoadCompleteCallBack += meshloader =>
            {
                TestVFX(meshloader);
            };
            
            //meshLoaderSocket.BeginLoadMesh();
            
        }

        
        private void TestVFX(BaseMeshLoader meshloader)
        {

            var firePoint = meshloader.FindTransformFromAvatarMesh("FirePoint");
            VFXBaseSpawnParam nextSpawnParams = ReferencePool.Acquire<VFXBaseSpawnParam>();
            nextSpawnParams.SpawnerEntityId = Id;
            nextSpawnParams.ReceiverEntityId = Id;
            nextSpawnParams.Position = firePoint.position.ToFloat3();
            nextSpawnParams.RotationQuat = firePoint.rotation.ToQuaternion();
            nextSpawnParams.Scale = Vector3.one.ToFloat3();
            nextSpawnParams.VFXIndexId = 1001;
            nextSpawnParams.VFXTypeId = (int)VFXType.Base;

            VFXUtils.SpawnVFX(nextSpawnParams);
        }

        public override void OnShow(object userData)
        {
            base.OnShow(userData);
            
            GameUtils.PlayerEntityId = Id;
            
        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new PlayerGameplayEntity(Entity);
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            // if (InputUtils.GetInput(EInputParam.kSprintBtnDownInput))
            // {
            //     Log.Error("sprint down");
            // }
            //
            // if (InputUtils.GetInput(EInputParam.kSprintBtnUpInput))
            // {
            //     Log.Error("sprint up");
            // }
        }

        public override void OnHide(bool isShutdown, object userData)
        {

            base.OnHide(isShutdown, userData);
        }


        public override void OnRecycle()
        {
            base.OnRecycle();
        }

        // private void OnMouseRayCast(object sender, GameEventArgs eventArgs)
        // {
        //     MouseRayCastEventArgs args = eventArgs as MouseRayCastEventArgs;
        //     var impactInfo = args.ImpactInfo;
        //     if (args.bIsHit)
        //     {
        //         var entity = GameEntry.Entity.GetEntity(impactInfo.HitEntityId);
        //         if (null == entity)
        //         {
        //             //Log.Info($"MouseHitEntity is null, entityId: {impactInfo.HitEntityId}");
        //         }
        //         else 
        //         {
        //             if (m_LastHitEntityLogic != null && entity.LogicInterface != m_LastHitEntityLogic)
        //             {
        //                 if(m_LastHitEntityLogic is IPointerHandler lastHitPointerHandler)
        //                     lastHitPointerHandler.PointerExit(new FPointerEventData()
        //                     {
        //                         ImpactInfo = impactInfo
        //                     });
        //             }
        //             
        //             if (entity.LogicInterface is IPointerHandler pointerHandler)
        //             {
        //                 if (m_LastHitEntityLogic != entity.LogicInterface)
        //                 {
        //                     pointerHandler.PointerEnter(new FPointerEventData()
        //                     {
        //                         ImpactInfo = impactInfo
        //
        //                     });
        //                 }
        //
        //                 if (InputUtils.GetKeyDown(EKeyCode.Mouse0))
        //                 {
        //                     pointerHandler.PointerDown(new FPointerEventData()
        //                     {
        //                         ImpactInfo = impactInfo
        //
        //                     });
        //                 }
        //                 
        //                 if (InputUtils.GetKeyDown(EKeyCode.Mouse1))
        //                 {
        //                     pointerHandler.PointerUp(new FPointerEventData()
        //                     {
        //                         ImpactInfo = impactInfo
        //
        //                     });
        //                 }
        //
        //
        //             }
        //             m_LastHitEntityLogic = entity.Logic;
        //
        //         }
        //     }
        //     else
        //     {
        //         if (m_LastHitEntityLogic != null)
        //         {
        //             if(m_LastHitEntityLogic is IPointerHandler lastHitPointerHandler)
        //                 lastHitPointerHandler.PointerExit(new FPointerEventData());
        //
        //             m_LastHitEntityLogic = null;
        //         }
        //     }
        // }
        
    }
}