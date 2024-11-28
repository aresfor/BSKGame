using System;
using Game.Core;
using Game.Math;
using GameFramework;
using GameFramework.Entity;

namespace Game.Gameplay
{
    /// <summary>
    /// 与Entity绑定的数据，子类需要在特定时机调用InitProperties
    /// </summary>
    [Serializable]
    public abstract class EntityData: IReference, IGameplayTagOwner
    {
        private int m_Id = 0;
        
        private int m_TypeId = 0;

        private int m_ResourceId = 0;
        
        private float3 m_InitPosition = float3.zero;
        
        private quaternion m_InitRotation = quaternion.identity;

        private IPropertyArr m_Properties = new PropertyArr();

        private FGameplayTagContainer m_GameplayTagContainer 
            = new FGameplayTagContainer();
        
        public EntityData()
        {
        }
        //在client show entity中调用
        //@TODO：这样调用流程有点乱，后面改
        public void Init()
        {
            OnInitialize();
        }
        
        protected virtual void OnInitialize()
        {
        }
        
        protected void InitProperties(int propertyId)
        {
            m_Properties.Initialize(propertyId);
        }
        
        protected abstract void OnClear();

        public void AddTag(string tag)
        {
            m_GameplayTagContainer.AddTag(tag);
        }

        public void RemoveTag(string tag)
        {
            m_GameplayTagContainer.RemoveTag(tag);
        }

        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact)
        {
            return m_GameplayTagContainer.HasTag(tag, checkType);
        }
        public void Clear()
        {
            OnClear();
            m_Properties.Reset();
            ClearAllTag();
        }
        
        public void ClearAllTag()
        {
            m_GameplayTagContainer.ClearAllTag();
        }
        
        /// <summary>
        /// 获取属性值
        /// </summary>
        public float GetProperty(EPropertyDefine propertyDefine)
        {
            return m_Properties.GetProperty(propertyDefine);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        public void SetProperty(EPropertyDefine propertyDefine, float value, bool triggerEvent = true)
        {
            m_Properties.SetProperty(propertyDefine, value, triggerEvent);
        }
        /// <summary>
        /// 获取可绑定属性
        /// </summary>
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
        /// 资源表Id， 可为空
        /// </summary>
        public int ResourceId
        {
            get
            {
                return m_ResourceId;
            }
            set
            {
                m_ResourceId = value;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public float3 InitPosition
        {
            get
            {
                return m_InitPosition;
            }
            set
            {
                m_InitPosition = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public quaternion InitRotation
        {
            get
            {
                return m_InitRotation;
            }
            set
            {
                m_InitRotation = value;
            }
        }

        
    }
}
