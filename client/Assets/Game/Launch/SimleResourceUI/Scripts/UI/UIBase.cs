using UnityEngine;

namespace Game.Client
{
    public class UIBase : MonoBehaviour
    {
        protected object Param;
        public virtual void OnEnter(object param)
        {
            Param = param;
        }
    }
}