using Game.Core;
using Game.Gameplay;
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
                Log.Info($"Health Change To: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
            
            m_Model.SetProperty(EPropertyDefine.Health, 110);

            GameplayTagHelper.InitializeGameplayTag();
            GameplayTagHelper.TagTree.AddTag("Entity.Effect.Frozen");
            GameplayTagHelper.TagTree.AddTag("Entity.Effect.Stun");
            GameplayTagHelper.TagTree.AddTag("Entity.Effect.Burn");
            GameplayTagHelper.SaveTagFile();
            
            m_Model.GameplayTagContainer.AddTag("Entity.Effect.Frozen");
            
            bool exact = m_Model.GameplayTagContainer.HasTag("Entity.Effect.Frozen", EGameplayTagCheckType.Exact);
            bool parent = m_Model.GameplayTagContainer.HasTag("Entity.Effect", EGameplayTagCheckType.Parent);
            
            Log.Info($"exact: {exact}, parent: {parent}");
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}