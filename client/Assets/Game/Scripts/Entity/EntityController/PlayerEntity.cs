using Game.Core;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class PlayerEntity:GameplayEntity
    {
        private PlayerEntityModel m_Model;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (PlayerEntityModel)userData;
            var healthBindable = m_Model.GetBindableProperty(EPropertyDefine.Health);
            healthBindable.Register((oldValue, newValue) =>
                Log.Info($"Health: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
            
            m_Model.SetProperty(EPropertyDefine.Health, 110);
        }
    }
}