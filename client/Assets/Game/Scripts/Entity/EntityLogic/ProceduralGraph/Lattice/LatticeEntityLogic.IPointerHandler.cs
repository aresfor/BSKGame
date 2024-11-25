
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework.Entity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public partial class LatticeEntityLogic:IPointerHandler
    {
        // public Action OnPointerEnterCall { get; private set; }
        // public Action OnPointerExitCall { get; private set; }
        // public Action OnPointerDownCall { get; private set; }
        // public Action OnPointerUpCall { get; private set; }

        private EntityPointerHandler m_EntityPointerHandler;
        public void InitPointerHandler()
        {
            m_EntityPointerHandler = new EntityPointerHandler(this.Entity);
        }

        private void RecyclePointerHandler()
        {

        }
        
        public bool PointerEnter(FPointerEventData eventData)
        {
            if (false == LatticeNode.IsAvailable)
                return false;
            
            if(false == m_EntityPointerHandler.PointerEnter(eventData))
                m_HoverSpriteRenderer.color = HoverColor;

            
            return true;
        }

        public bool PointerExit(FPointerEventData eventData)
        {
            if (false == LatticeNode.IsAvailable)
                return false;
            m_HoverSpriteRenderer.color = DefaultHoverColor;
            m_EntityPointerHandler.PointerExit(eventData);
            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            if (false == LatticeNode.IsAvailable)
                return false;
            if (GameUtils.SelectedRoleEntityLogic != null)
            {
                GameUtils.SelectedRoleEntityLogic.MoveToDestination(LatticeNode.WorldPosition
                    , LatticeNode.Owner);
                
                //@TODO: Debug继承到gameplay
                List<float3> debugPoints = ListPool<float3>.Get();
                if (LatticeNode.Owner.AStar(GameUtils.SelectedRoleEntityLogic.Position.ToFloat3()
                        , LatticeNode.WorldPosition, debugPoints))
                {
                    foreach (var point in debugPoints)
                    {
                        DrawGizmos.Instance.DrawSphereGizmos(point, 0.3f, Color.green, 5.0f);
                    }
                }
                debugPoints.Clear();
                ListPool<float3>.Release(debugPoints);
                
            }
            return  m_EntityPointerHandler.PointerDown(eventData);
        }

        public bool PointerUp(FPointerEventData eventData)
        {
            if (false == LatticeNode.IsAvailable)
                return false;
            return m_EntityPointerHandler.PointerDown(eventData);;
        }
    }
}