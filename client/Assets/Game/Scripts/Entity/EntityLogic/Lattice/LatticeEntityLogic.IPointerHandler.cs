
using System.Collections.Generic;
using Game.Core;
using GameFramework.Entity;
using UnityEngine.EventSystems;

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
        
        public bool PointerEnter()
        {
            if(false == m_EntityPointerHandler.PointerEnter())
                m_HoverSpriteRenderer.color = HoverColor;

            return true;
        }

        public bool PointerExit()
        {
            m_HoverSpriteRenderer.color = DefaultHoverColor;
            m_EntityPointerHandler.PointerExit();
            return true;
        }

        public bool PointerDown()
        {
            
            return  m_EntityPointerHandler.PointerDown();
        }

        public bool PointerUp()
        {
            
            return m_EntityPointerHandler.PointerDown();;
        }
    }
}