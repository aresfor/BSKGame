
using System.Collections.Generic;
using Game.Core;
using Game.Math;
using GameFramework.Entity;
using UnityEngine;
using UnityEngine.EventSystems;
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
            if(false == m_EntityPointerHandler.PointerEnter(eventData))
                m_HoverSpriteRenderer.color = HoverColor;

            
            return true;
        }

        public bool PointerExit(FPointerEventData eventData)
        {
            m_HoverSpriteRenderer.color = DefaultHoverColor;
            m_EntityPointerHandler.PointerExit(eventData);
            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            if (GameUtils.SelectedRoleEntityLogic != null)
            {
                GameUtils.SelectedRoleEntityLogic.MoveToDestination(LatticeNode.WorldPosition);
                
                //@TODO: Debug继承到gameplay
                List<float3> debugPoints = ListPool<float3>.Get();
                if (LatticeNode.Owner.BFS(GameUtils.SelectedRoleEntityLogic.Position.ToFloat3()
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
            
            return m_EntityPointerHandler.PointerDown(eventData);;
        }
    }
}