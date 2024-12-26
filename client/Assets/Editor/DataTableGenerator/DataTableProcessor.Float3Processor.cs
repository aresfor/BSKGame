
using System.IO;
using Game.Math;
using UnityEngine;

namespace Game.Client.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Float3Processor : GenericDataProcessor<float3>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "float3";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "float3",
                    "Game.Math.float3"
                };
            }

            public override float3 Parse(string value)
            {
                string[] splitedValue = value.Split(',');
                return new float3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                float3 f3 = Parse(value);
                binaryWriter.Write(f3.x);
                binaryWriter.Write(f3.y);
                binaryWriter.Write(f3.z);
            }
        }
    }
}
