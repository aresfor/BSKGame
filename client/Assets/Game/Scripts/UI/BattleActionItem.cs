using Game.Core;
using UnityEngine.EventSystems;

namespace Game.Client
{
    public class BattleActionItem:BaseMonoBehaviour, IController
    {
        //inspector
        public CustomButton Button;
        public IBattleAction BattleAction;

        //private
        private BattleMainModel m_Model;

        public virtual void OnShow()
        {
            m_Model = this.GetModel<BattleMainModel>();

            Button.OnPointerEnterEvent.AddListener(OnPointerEnter);
            Button.OnPointerExitEvent.AddListener(OnPointerExit);

        }


        private void OnPointerEnter(PointerEventData eventdata)
        {
            m_Model.CurrentDisplayBattleActionItem.Value = this;
            
        }

        private void OnPointerExit(PointerEventData eventdata)
        {
            m_Model.CurrentDisplayBattleActionItem.Value = null;
        }
        
        public virtual void OnRecycle()
        {
            Button.OnPointerEnterEvent.RemoveListener(OnPointerEnter);
            Button.OnPointerExitEvent.RemoveListener(OnPointerExit);
        }
        
        public IArchitecture GetArchitecture()
        {
            return BattleMainArchitecture.Interface;
        }
        
    }
}