using UnityEditor;
using UnityEngine;

namespace Game.Client
{    
    [CustomEditor(typeof(MonoBoard))]

    public class MonoBoardEditor:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("重新生成"))
            {
                ((MonoBoard)serializedObject.targetObject).Generate();
            }
            
            if (GUILayout.Button("清理"))
            {
                ((MonoBoard)serializedObject.targetObject).ClearBoard();
            }
        }
    }
}