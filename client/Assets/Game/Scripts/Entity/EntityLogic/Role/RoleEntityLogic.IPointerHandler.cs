using Game.Core;
using Game.Math;
using UnityEngine;

namespace Game.Client
{
    public partial class RoleEntityLogic: IPointerHandler
    {
        private bool m_Selected;
        public bool PointerEnter(FPointerEventData eventData)
        {
            CachedTransform.localScale = Vector3.one * 2.0f;
            
            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            m_Selected = !m_Selected;
            foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            {
                material.color = m_Selected? Color.green : Color.white;
            }
            //@TEMP:
            if(m_Selected)
                GameUtils.SelectedRoleEntityLogic = this;
            
            return true;
        }

        public bool PointerUp(FPointerEventData eventData)
        {
            return true;
        }
        
        public bool PointerExit(FPointerEventData eventData)
        {
            CachedTransform.localScale = Vector3.one * 1.0f;
            return true;

        }

        
    }
}