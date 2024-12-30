using System;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Event;
using GameFramework.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
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
            
            //属性
            var healthBindable = m_Model.GetBindableProperty((int)EPropertyDefine.Health);
            healthBindable.Register((oldValue, newValue) =>
                Log.Info($"Health Change To: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
            
            m_Model.SetProperty((int)EPropertyDefine.Health, 110);
            
            //Tag
            AddTag("Entity.Effect.Frozen");
            bool exact = HasTag("Entity.Effect.Burn", EGameplayTagCheckType.Exact);
            bool parent = HasTag("Entity.Effect", EGameplayTagCheckType.Parent);
            Log.Info($"exact: {exact}, parent: {parent}");
            
            //vfx/特效用例
            VFXBaseSpawnParam spawnParam = ReferencePool.Acquire<VFXBaseSpawnParam>();
            spawnParam.SpawnerEntityId = Id;
            spawnParam.ReceiverEntityId = Id;
            spawnParam.Position = Vector3.forward.ToFloat3() * 5.0f;
            spawnParam.Scale = Vector3.one.ToFloat3();
            spawnParam.VFXIndexId = 1001;
            spawnParam.VFXTypeId = (int)VFXType.Base;
            var vfxSerialId = VFXUtils.SpawnVFX(spawnParam);

            //"销毁"特效
            VFXUtils.DeSpawnVFX(vfxSerialId);

            
            //Entity Prefab上配置插槽，用于独立功能
            //MeshLoaderSocket用于模型加载
            var meshLoaderSocket = FindLogicSocket<BaseMeshLoaderLogicSocket>();
            meshLoaderSocket.MeshLoadCompleteCallBack += meshloader =>
            {
                //模型加载完之后生成一个测试特效
                TestVFX(meshloader);
            };
            meshLoaderSocket.BeginLoadMesh();
            
            
            //框架功能示例
            //全局事件
            //GF全局事件，基于Id
            // GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId
            //    , (sender, args) => Log.Info("ShowEntitySuccess"));
            //GameEntry.Event.FireNow();
            //GameEntry.Event.Unsubscribe();
            //另一种全局事件，基于结构体/类，如下，接受所有参数类型为int的的事件
            //TypeEventSystem.Global.Register<int>(i => Log.Info("int received"));
            
            //加载GameObject
            var goId = ResourceExtension.GenerateGameObjectSerialId();
            //GameEntry.Resource.Instantiate(goId, "Model/PlayerModel"
            //    , null, null);
            //GameEntry.Resource.UnInstantiate(goId);
            
            //Entity
            //获取Entity，这个Id是创建Entity时候的Id
            var entity = GameEntry.Entity.GetEntity(Id);
            var entityLogic = entity.Logic;
            var entityLogicInterface = entity.LogicInterface;
            var gameplayEntity = ((GameEntityLogic)entityLogic)?.GameplayEntity;
            //创建Entity，只用ShowGameplayEntity，不要用其他ShowEntity接口
            //@TODO:之后不再使用xxxEntityModel来包装数据，全部转到Entity->Component中去
            // var entityId = EntityId.GenerateSerialId();
            // var playerRoleModel = new RoleEntityModel()
            // {
            //     Id = entityId,
            //     TypeId = Constant.Entity.PlayerTypeId,
            //     //@TEMP:
            //     ResourceId = 10000
            // };
            // GameEntry.Entity.ShowGameplayEntity("Player", playerRoleModel);
            // //隐藏一个实体
            // GameEntry.Entity.HideEntity(entityId);
            

        }

        
        private void TestVFX(BaseMeshLoader meshloader)
        {
            //挂点，在模型预制体上配置，和Entity Prefab不同，挂点信息都是配置在模型预制体上的
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
            
            //GameUtils.PlayerEntityId = Id;
            
        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new PlayerGameplayEntity(Entity);
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            //利用输入类进行输入逻辑处理
            if (InputUtils.GetKeyDown(EKeyCode.A))
            {
                Log.Info("Press A");
            }
            
            if (InputUtils.GetInput(EInputParam.kSprintBtnDownInput))
            {
                Log.Info("sprint down");
            }
            
            if (InputUtils.GetInput(EInputParam.kSprintBtnUpInput))
            {
                Log.Info("sprint up");
            }
        }

        public override void OnHide(bool isShutdown, object userData)
        {

            base.OnHide(isShutdown, userData);
        }


        public override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}