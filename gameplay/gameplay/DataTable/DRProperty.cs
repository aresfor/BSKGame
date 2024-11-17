using Game.Core;
using GameFramework.DataTable;

namespace Game.Gameplay;

public class DRProperty:IDataRow
{

    private int m_Id = 0;

    private float[] m_Properties = new float[Length];
    public int Id { get=>m_Id; }
    public static int Length => (int)EPropertyDefine.Max;
    public float this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of range.");
            }
            return m_Properties[index];
        }
        set
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of range.");
            }
            m_Properties[index] = value;
        }
    }
    
    public bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = dataRowString.Split('\t');
        int columnLength = columnStrings.Length;
        for (int i = 0; i < columnLength; i++)
        {
            columnStrings[i] = columnStrings[i].Trim('\"');
        }

        int index = 0;
        //#列
        index++;
        //Id
        m_Id = int.Parse(columnStrings[index++]);
        //备注
        index++;
        
        //属性值，@TODO： 之后考虑接luban，不然现在同时要维护属性枚举和表格
        if ((columnLength-3) != Length)
        {
            Log.Error("property datatable column length not equal to property define, check");
            return false;
        }
        
        for (int i = 0; i < Length; ++i)
        {
            float value = float.Parse(columnStrings[index++]);
            m_Properties[i] = value;
        }
        return true;
    }

    public bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        throw new NotImplementedException();
    }
}