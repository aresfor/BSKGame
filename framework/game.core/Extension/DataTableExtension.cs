using Game.Math;

namespace GameFramework.Runtime;

public static partial class DataTableExtension
{
    public static readonly char[] DataSplitSeparators = {'\t'};
    public static readonly char[] DataTrimSeparators = {'\"'};
    
    private const string DataRowClassPrefixName = "Game.Gameplay.DR";
        
    public static float3 Parsefloat3(string value)
    {
        var splitValue = value.Split(',');
        return new float3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));
    }

    public static float3 Readfloat3(this BinaryReader binaryReader)
    {
        return new float3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
    }
}