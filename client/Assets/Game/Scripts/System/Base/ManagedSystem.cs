using Game.Core;

namespace Game.Client
{
    public abstract class ManagedSystem: AbstractSystem
    {
        protected override void OnInit()
        {
            GameEntry.BuiltinData.AddSystem(this);
        }

        protected override void OnDeinit()
        {
            GameEntry.BuiltinData.RemoveSystem(this);
            base.OnDeinit();
        }
    }
}