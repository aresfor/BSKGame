using System;
using System.Collections.Generic;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using GameFramework.DataTable;

namespace Game.Core
{

    public class PropertyArr : IPropertyArr
    {
        //private IBindableProperty<float>[] m_Properties = new IBindableProperty<float>[Length];
        private Dictionary<EPropertyDefine, IBindableProperty<float>> m_Properties
            = new Dictionary<EPropertyDefine, IBindableProperty<float>>();

        private DRProperty m_OriginProperties;

        private int m_PropertyId;

        private const string MaxPropertyPrefix = "Max";
        private static string[] m_PropertiesName = new string[Length];
        private static HashSet<EPropertyDefine> m_HasMaxValueSet = new HashSet<EPropertyDefine>();

        public void Initialize(int propertyId, bool triggerEvent = true)
        {
            //缓存enum name
            if (string.IsNullOrEmpty(m_PropertiesName[0]))
            {
                for (int i = Length - 1; i > 0; --i)
                {
                    m_PropertiesName[i] = ((EPropertyDefine)i).ToString();
                    if (HasMaxValue(i - 1))
                    {
                        m_HasMaxValueSet.Add((EPropertyDefine)(i - 1));
                    }
                }
            }

            m_PropertyId = propertyId;
            IDataTableManager dataTableManager = GameFrameworkEntry.GetModule<IDataTableManager>();
            IDataTable<DRProperty> dtProperty = dataTableManager.GetDataTable<DRProperty>();
            if (null == dtProperty)
            {
                Logs.Error($"get datatable:{nameof(DRProperty)} fail");
                return;
            }

            m_OriginProperties = dtProperty.GetDataRow(propertyId);
            if (null == m_OriginProperties)
            {
                Logs.Error($"get drProperty fail, Id:{propertyId}");
            }

            InitProperties(triggerEvent);
        }


        private bool HasMaxValue(int propertyIndex)
        {
            //不合法
            if (propertyIndex < 0 || propertyIndex > Length - 1)
            {
                return false;
            }

            if (m_PropertiesName[propertyIndex + 1].StartsWith(MaxPropertyPrefix))
            {
                return true;
            }

            return false;
        }

        private void InitProperties(bool triggerEvent)
        {
            if (null == m_OriginProperties)
            {
                Logs.Error($"origin properties is null, propertyId:{m_PropertyId}, properties init with 0");
            }

            if (m_Properties.Count <= 0)
            {
                for (int i = 0; i < Length; ++i)
                {
                    m_Properties[(EPropertyDefine)i] = new BindableProperty<float>();
                }
            }

            if (triggerEvent)
            {
                m_Properties[EPropertyDefine.Health].Value = m_OriginProperties.Health;
                m_Properties[EPropertyDefine.MaxHealth].Value = m_OriginProperties.MaxHealth;
                m_Properties[EPropertyDefine.Mana].Value = m_OriginProperties.Mana;
                m_Properties[EPropertyDefine.MaxMana].Value = m_OriginProperties.MaxMana;
                m_Properties[EPropertyDefine.Armor].Value = m_OriginProperties.Armor;
                m_Properties[EPropertyDefine.MaxArmor].Value = m_OriginProperties.MaxArmor;
            }
            else
            {
                m_Properties[EPropertyDefine.Health].SetValueWithoutEvent(m_OriginProperties.Health);
                m_Properties[EPropertyDefine.MaxHealth].SetValueWithoutEvent(m_OriginProperties.MaxHealth);
                m_Properties[EPropertyDefine.Mana].SetValueWithoutEvent(m_OriginProperties.Mana);
                m_Properties[EPropertyDefine.MaxMana].SetValueWithoutEvent(m_OriginProperties.MaxMana);
                m_Properties[EPropertyDefine.Armor].SetValueWithoutEvent(m_OriginProperties.Armor);
                m_Properties[EPropertyDefine.MaxArmor].SetValueWithoutEvent(m_OriginProperties.MaxArmor);
            }

        }

        public float GetProperty(EPropertyDefine propertyDefine)
        {
            return m_Properties[propertyDefine].Value;
        }

        public void SetProperty(EPropertyDefine propertyDefine, float value, bool triggerEvent = true)
        {
            //@TODO: 最小值限制
            value = math.max(0, value);

            //最大值限制
            if (m_HasMaxValueSet.Contains(propertyDefine))
            {
                value = math.min(value, GetProperty(propertyDefine + 1));
            }

            if (triggerEvent)
                m_Properties[propertyDefine].Value = value;
            else
            {
                m_Properties[propertyDefine].SetValueWithoutEvent(value);
            }
        }

        public IReadonlyBindableProperty<float> this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException($"Index {index} is out of range.");
                }

                return m_Properties[(EPropertyDefine)index];
            }
        }

        public void Reset()
        {
            m_PropertyId = 0;
            foreach (var bindableProperty in m_Properties.Values)
            {
                bindableProperty.UnRegisterAll();
            }

            InitProperties(false);
        }

        private static int Length => IPropertyArr.Length;

    }
}