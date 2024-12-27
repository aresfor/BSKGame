using Game.Core;

namespace Game.Client
{
    public abstract class ManagedSystem: AbstractSystem
    {
        protected override void OnInit()
        {
            //@TODO:接入到GameStarter中
            
            
            //UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>().AddSystem(this);
        }

        protected override void OnDeinit()
        {
            //UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>().RemoveSystem(this);
            base.OnDeinit();
        }
    }
}