using Game.Core;
using GameFramework;

namespace Game.Client
{
    public abstract class EntityModel:EntityData, IReference
    {
        public EntityModel()
        {
            
        }
        protected abstract void OnClear();

        public void Clear() => OnClear();
    }
}