using System;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityEngine;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class PlayerEntityLogic:GameEntityLogic
    {
        private RoleEntityModel m_Model;
         
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
            
            meshLoaderSocket.BeginLoadMesh();
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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TestVFX(FindLogicSocket<BaseMeshLoaderLogicSocket>().AvatarMeshLoader);
            }
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}