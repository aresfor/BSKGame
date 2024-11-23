using UnityEngine;

namespace Game.Client
{
    public partial class RoleEntityLogic: IPointerHandler
    {
        private bool m_PointerDown;
        public bool PointerEnter()
        {
            CachedTransform.localScale = Vector3.one * 2.0f;
            
            return true;
        }

        public bool PointerDown()
        {
            m_PointerDown = !m_PointerDown;
            foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            {
                material.color = m_PointerDown? Color.green : Color.white;
            }
            
            return true;
        }

        public bool PointerUp()
        {
            return true;
        }
        
        public bool PointerExit()
        {
            CachedTransform.localScale = Vector3.one * 1.0f;
            return true;

        }
    }
}