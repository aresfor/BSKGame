using Game.Core;
using Game.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public partial class RoleEntityLogic: GameEntityLogic
    {

        private RoleEntityModel m_RoleEntityModel;
        private MeshRenderer m_MainMeshRenderer;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitViewExtend();
            InitNavigationAgent();
            
            m_RoleEntityModel = EntityData as RoleEntityModel;
            var meshLoaderSocket = FindLogicSocket<BaseMeshLoaderLogicSocket>();
            meshLoaderSocket.MeshPrefabName = m_RoleEntityModel.RoleData.Model;
            meshLoaderSocket.MeshLoadCompleteCallBack += OnMeshLoadComplete;
            meshLoaderSocket.BeginLoadMesh();
            
            var healthBindable = m_RoleEntityModel.GetBindableProperty(EPropertyDefine.Health);
            healthBindable.Register((oldValue, newValue) =>
                Log.Info($"Health Change To: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
        }

        private void OnMeshLoadComplete(BaseMeshLoader meshLoader)
        {
            m_MainMeshRenderer = GetMainMesh().GetComponentInChildren<MeshRenderer>();
            FindLogicSocket<ColliderBindLogicSocket>()?.CollectAllCollider();
        }
        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new RoleGameplayEntity(this.Entity);
        }
        

        public override void OnShow(object userData)
        {
            base.OnShow(userData);

        }

        private void AttachTile()
        {
            GraphUtils.GetGraph<IGraph>().WorldToGraph(ViewPosition, out var node);
            //@TODO: 这里依赖类型了，之后改
            var tileNode = (TileMapGraphNode)node;
            var tileEntity = GameEntry.Entity.GetEntity(tileNode.Value.Entity.Id);
            if (tileEntity == null)
            {
                Log.Error("Tile entity null, check");
            }
            GameEntry.Entity.AttachEntity(this.Id, tileNode.Value.Entity.Id);
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateNavigation(elapseSeconds, realElapseSeconds);
        }

        public override void OnRecycle()
        {
            RecycleViewExtend();
            base.OnRecycle();
        }

        
    }
}