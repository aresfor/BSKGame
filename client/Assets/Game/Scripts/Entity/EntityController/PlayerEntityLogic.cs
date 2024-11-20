using Game.Core;
using Game.Gameplay;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class PlayerEntityLogic:GameEntityLogic
    {
        private PlayerEntityModel m_Model;
         
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (PlayerEntityModel)userData;
            var healthBindable = m_Model.GetBindableProperty(EPropertyDefine.Health);
            healthBindable.Register((oldValue, newValue) =>
                Log.Info($"Health Change To: {newValue}, Old:{oldValue}")).UnRegisterWhenDisabled(this);
            
            m_Model.SetProperty(EPropertyDefine.Health, 110);
            
            AddTag("Entity.Effect.Frozen");
            
            bool exact = HasTag("Entity.Effect.Burn", EGameplayTagCheckType.Exact);
            bool parent = HasTag("Entity.Effect", EGameplayTagCheckType.Parent);
            
            Log.Info($"exact: {exact}, parent: {parent}");
        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new PlayerGameplayEntity(Entity);
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}