using Game.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

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
            
            
            m_RoleEntityModel = EntityData as RoleEntityModel;
            var meshLoaderSocket = FindLogicSocket<BaseMeshLoaderLogicSocket>();
            meshLoaderSocket.MeshPrefabName = m_RoleEntityModel.RoleData.Model;
            meshLoaderSocket.MeshLoadCompleteCallBack += OnMeshLoadComplete;
            meshLoaderSocket.BeginLoadMesh();
        }

        private void OnMeshLoadComplete(BaseMeshLoader meshLoader)
        {
            m_MainMeshRenderer = GetMainMesh().GetComponentInChildren<MeshRenderer>();
        }
        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new RoleGameplayEntity(this.Entity);
        }
        

        public override void OnShow(object userData)
        {
            base.OnShow(userData);
            
            GameEntry.Entity.AttachEntity(this.Id, m_RoleEntityModel.BelongLatticeId);
        }
        

        public override void OnRecycle()
        {
            RecycleViewExtend();
            base.OnRecycle();
        }

        
    }
}