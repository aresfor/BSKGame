using System;
using Game.Gameplay;
using Game.Math;

namespace Game.Core
{
    /// <summary>
    /// 与Entity绑定的数据，子类需要在特定时机调用InitProperties
    /// </summary>
    [Serializable]
    public abstract class EntityData
    {
        private int m_Id = 0;
        
        private int m_TypeId = 0;
        
        private float3 m_Position = float3.zero;
        
        private quaternion m_Rotation = quaternion.identity;

        private IPropertyArr m_Properties = new PropertyArr();
        
        public EntityData()
        {
            
        }
        public void Init(int entityId, int typeId)
        {
            m_Id = entityId;
            m_TypeId = typeId;
            OnInitialize();
        }
        protected virtual void OnInitialize()
        {
        }
        protected void InitProperties(int propertyId)
        {
            m_Properties.Initialize(propertyId);
        }

        public float GetProperty(EPropertyDefine propertyDefine)
        {
            return m_Properties.GetProperty(propertyDefine);
        }

        public void SetProperty(EPropertyDefine propertyDefine, float value, bool triggerEvent = true)
        {
            m_Properties.SetProperty(propertyDefine, value, triggerEvent);
        }
        public IReadonlyBindableProperty<float> GetBindableProperty(EPropertyDefine propertyDefine)
        {
            return m_Properties[(int)propertyDefine];
        }
        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
            set => m_Id = value;
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get
            {
                return m_TypeId;
            }
            set => m_TypeId = value;
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public float3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public quaternion Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }
    }
}
