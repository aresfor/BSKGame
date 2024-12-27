using Game.Core;
using GameFramework;

namespace Game.Gameplay
{
    public abstract class EntityModel:EntityData
    {
        private IPropertyArr m_Properties = new PropertyArr();

        public EntityModel()
        {
        }

        protected override void InitProperties(int propertyId)
        {
            base.InitProperties(propertyId);
            m_Properties.Initialize(propertyId);

        }

        public override float GetProperty(int propertyDefine)
        {
            return m_Properties.GetProperty((EPropertyDefine)propertyDefine);
        }

        public override void SetProperty(int propertyDefine, float value, bool triggerEvent = true)
        {
            m_Properties.SetProperty((EPropertyDefine)propertyDefine, value, triggerEvent);
        }

        public override IReadonlyBindableProperty<float> GetBindableProperty(int propertyDefine)
        {
            return m_Properties[(int)propertyDefine];
        }

        protected override void OnClear()
        {
            m_Properties.Reset();
        }
    }
}