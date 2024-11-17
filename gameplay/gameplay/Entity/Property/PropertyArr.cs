using Game.Gameplay;
using Game.Math;
using GameFramework;
using GameFramework.DataTable;

namespace Game.Core;

public class PropertyArr:IPropertyArr
{
    private IBindableProperty<float>[] m_Properties = new IBindableProperty<float>[Length];

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
            for (int i = Length -1; i > 0; --i)
            {
                m_PropertiesName[i] = ((EPropertyDefine)i).ToString();
                if (HasMaxValue(i-1))
                {
                    m_HasMaxValueSet.Add((EPropertyDefine)(i-1));
                }
            }
        }
        
        m_PropertyId = propertyId;
        IDataTableManager dataTableManager = GameFrameworkEntry.GetModule<IDataTableManager>();
        IDataTable<DRProperty> dtProperty = dataTableManager.GetDataTable<DRProperty>();
        if (null == dtProperty)
        {
            Log.Error($"get datatable:{nameof(DRProperty)} fail");
            return;
        }

        m_OriginProperties = dtProperty.GetDataRow(propertyId);
        if (null == m_OriginProperties)
        {
            Log.Error($"get drProperty fail, Id:{propertyId}");
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
        bool bHasOriginValue = true;
        if (null == m_OriginProperties)
        {
            Log.Error($"origin properties is null, propertyId:{m_PropertyId}, properties init with 0");
            bHasOriginValue = false;
        }
        
        
        for (int i = 0;i<Length;++i)
        {
            if (m_Properties[i] == null)
            {
                m_Properties[i] = new BindableProperty<float>();
            }
            
            if (bHasOriginValue)
            {
                if (triggerEvent)
                {
                    m_Properties[i].Value = m_OriginProperties[i];
                }
                else
                {
                    m_Properties[i].SetValueWithoutEvent(m_OriginProperties[i]);
                }
            }
            else
            {
                if (triggerEvent)
                {
                    m_Properties[i].Value = 0;
                }
                else
                {
                    m_Properties[i].SetValueWithoutEvent(0);
                }
            }
        }
    }
    public float GetProperty( EPropertyDefine propertyDefine)
    {
        return m_Properties[(int)propertyDefine].Value;
    }

    public void SetProperty( EPropertyDefine propertyDefine, float value, bool triggerEvent = true)
    {
        //@TODO: 最小值限制
        
        //最大值限制
        if (m_HasMaxValueSet.Contains(propertyDefine))
        {
            value = math.min(value, GetProperty(propertyDefine + 1));
        }
        
        if(triggerEvent)
            m_Properties[(int)propertyDefine].Value = value;
        else
        {
            m_Properties[(int)propertyDefine].SetValueWithoutEvent(value);
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
            return m_Properties[index];
        }
    }

    public void Reset()
    {
        m_PropertyId = 0;
        foreach (var bindableProperty in m_Properties)
        {
            bindableProperty.UnRegisterAll();
        }
        
        InitProperties(false);
    }

    private static int Length => IPropertyArr.Length;

}