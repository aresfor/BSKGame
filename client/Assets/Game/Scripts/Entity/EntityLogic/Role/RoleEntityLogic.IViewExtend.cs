using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public partial class RoleEntityLogic: IViewExtend
    {
        private BaseMeshLoaderLogicSocket m_MeshLoaderLogicSocket;
        private void InitViewExtend()
        {
            m_MeshLoaderLogicSocket = FindLogicSocket<BaseMeshLoaderLogicSocket>();
        }
        public Transform GetTransform(string tsName)
        {
            return m_MeshLoaderLogicSocket.FindTransformFromAvatarMesh(tsName);
        }

        public Transform GetLogicTransform()
        {
            return this.transform;
        }

        public Transform GetMainMesh() => m_MeshLoaderLogicSocket.DefaultMeshSocketTs.GetChild(0).Find(IViewExtend.MainMeshName);

        private void RecycleViewExtend()
        {
            m_MeshLoaderLogicSocket = null;
        }
        
    }
}