using Game.Core;

namespace Game.Client
{
    public abstract class EntityModel:EntityData, IModel
    {
        protected EntityModel(int entityId, int typeId) : base(entityId, typeId)
        {
        }

        private IArchitecture m_Architecture;

        public IArchitecture GetArchitecture() => m_Architecture;

        public void SetArchitecture(IArchitecture architecture)
        {
            m_Architecture = architecture;
        }
        
        void ICanInit.Init() => OnInit();
        public void Deinit() => OnDeinit();

        protected virtual void OnDeinit()
        {
        }

        protected abstract void OnInit();

        public bool Initialized { get; set; }
    }
}