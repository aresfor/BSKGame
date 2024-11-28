using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using UnityEngine;

namespace Game.Client
{
    public partial class TileNodeEntityLogic:IPointerHandler
    {
        
        private EntityPointerHandler m_EntityPointerHandler;
        public Color DefaultHoverColor = Color.white;
        public Color HoverColor = Color.red;
        public SpriteRenderer HoverRenderer;
        public void InitPointerHandler()
        {
            m_EntityPointerHandler = new EntityPointerHandler(this.Entity);
        }

        private void RecyclePointerHandler()
        {
        }
        
        public bool PointerEnter(FPointerEventData eventData)
        {
            if (false == m_Model.Node.IsAvailable)
                return false;

            if (false == m_EntityPointerHandler.PointerEnter(eventData))
            {
                Hover(true);
                //((TileGraphNode)m_Model.Node).SetColor( HoverColor);
            }

            
            
            return true;
        }

        public void Hover(bool enable)
        {
            HoverRenderer.color = enable ? HoverColor : DefaultHoverColor;
            
        }

        public bool PointerExit(FPointerEventData eventData)
        {
            if (false == m_Model.Node.IsAvailable)
                return false;
            Hover(false);
            //((TileGraphNode)m_Model.Node).SetColor( DefaultHoverColor);
            m_EntityPointerHandler.PointerExit(eventData);
            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            if (false == m_Model.Node.IsAvailable)
                return false;
            if (GameUtils.SelectedEntityId != 0)
            {
                var selectedEntity = GameEntry.Entity.GetEntity(GameUtils.SelectedEntityId);
                if (selectedEntity != null && selectedEntity.Logic is RoleEntityLogic roleEntityLogic)
                {
                    roleEntityLogic.MoveToDestination(m_Model.Node.WorldPosition, m_Model.Node.Owner);

                    //@TODO: Debug继承到gameplay
                    using var debugPointData = new FPoolWrapper<List<float3>, float3>();
                    if (m_Model.Node.Owner.AStar(roleEntityLogic.Position.ToFloat3()
                            , m_Model.Node.WorldPosition, debugPointData.Value))
                    {
                        foreach (var point in debugPointData.Value)
                        {
                            DrawGizmos.Instance.DrawSphereGizmos(point, 0.3f, Color.green, 5.0f);
                        }
                    }

                }
            }

            return  m_EntityPointerHandler.PointerDown(eventData);
        }

        public bool PointerUp(FPointerEventData eventData)
        {
            if (false == m_Model.Node.IsAvailable)
                return false;
            return m_EntityPointerHandler.PointerDown(eventData);;
        }
    }
}