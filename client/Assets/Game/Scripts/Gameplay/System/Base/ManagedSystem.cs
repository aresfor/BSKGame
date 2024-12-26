using Game.Core;

namespace Game.Client
{
    public abstract class ManagedSystem: AbstractSystem
    {
        protected override void OnInit()
        {
            //UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>().AddSystem(this);
        }

        protected override void OnDeinit()
        {
            //UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>().RemoveSystem(this);
            base.OnDeinit();
        }
    }
}