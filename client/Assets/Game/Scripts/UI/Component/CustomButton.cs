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
        public UnityEvent<PointerEventData> OnPointerClieckEvent;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            OnPointerEnterEvent?.Invoke(eventData);
            
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnPointerExitEvent?.Invoke(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            OnPointerClieckEvent?.Invoke(eventData);
        }
    }
}