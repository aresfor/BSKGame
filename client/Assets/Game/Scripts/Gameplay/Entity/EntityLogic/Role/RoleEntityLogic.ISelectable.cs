using UnityEngine;

namespace Game.Client
{
    public partial class RoleEntityLogic:ISelectable
    {
        public bool Selected { get; private set; }
        public void OnSelect()
        {
            Selected = true;
            
            foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            {
                material.color = Color.green;
            }
        }

        public void OnDeSelect()
        {
            Selected = false;
            
            foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            {
                material.color = Color.white;
            }
        }
    }
}