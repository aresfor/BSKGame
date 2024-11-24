using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Client
{
    public class CustomButton: Button
    {
        public UnityEvent<PointerEventData> OnPointerEnterEvent;
        public UnityEvent<PointerEventData> OnPointerExitEvent;
        private bool m_bIsPointerInside = false;
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            m_bIsPointerInside = true;
            OnPointerEnterEvent?.Invoke(eventData);
            
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            m_bIsPointerInside = false;
            OnPointerExitEvent?.Invoke(eventData);
        }
    }
}