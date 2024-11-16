using UnityEditor;
using UnityEngine;

namespace Game.Client
{
    [CustomEditor(typeof(MonoMeshGenerator))]
    public class MonoMeshGeneratorEditor:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("重新生成"))
            {
                ((MonoMeshGenerator)serializedObject.targetObject).Generate();
            }
        }
    }
}