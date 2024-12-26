using System;
using Game.Core;
using Game.Math;
using GameFramework;
using UnityEngine;

namespace Game.Client
{
    public partial class RoleEntityLogic: IPointerHandler
    {
        //private bool m_Selected;
        public bool PointerEnter(FPointerEventData eventData)
        {
            //CachedTransform.localScale = Vector3.one * 2.0f;
            
            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            //m_Selected = !m_Selected;
            // foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            // {
            //     //material.color = m_Selected? Color.green : Color.white;
            //     material.color = Color.green;
            // }
            
            
            //@TEMP:
            // if (m_Selected)
            // {
            //     if (false == GameUtils.BattleManager.ContainAnyAction())
            //     {
            //         var selectEntityAction = ReferencePool.Acquire<SaveSelectEntityAction>();
            //         selectEntityAction.SelectEntityActionType = ESelectEntityActionType.SelectOwner;
            //         selectEntityAction.SelectedEntityId = this.Id;
            //         GameUtils.BattleManager.PushAction(selectEntityAction);
            //         //GameUtils.SelectedEntityId = this.Id;
            //     }
            //     
            // }
            
            return true;
        }

        public bool PointerUp(FPointerEventData eventData)
        {
            //if (m_Selected)
            // {
            //     foreach (var material in m_MeshLoaderLogicSocket.AvatarMeshLoader.AllMaterials)
            //     {
            //         material.color = Color.white;
            //     }
            // }
            return true;
        }
        
        public bool PointerExit(FPointerEventData eventData)
        {
            
            //CachedTransform.localScale = Vector3.one * 1.0f;
            return true;

        }


        
    }
}