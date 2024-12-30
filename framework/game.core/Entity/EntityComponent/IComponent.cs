using GameFramework;

namespace Game.Core
{
    public interface IComponent
    {
    
    }

    public abstract class BaseComponent : IComponent, IReference
    {
        public void Clear()
        {
            OnClear();
        }

        protected abstract void OnClear();
    }
}

