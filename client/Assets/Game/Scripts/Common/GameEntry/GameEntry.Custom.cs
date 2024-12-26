using GameFramework;
using UnityEngine;

namespace Game.Client
{
    public partial class GameEntry
    {
        // public static BuiltinDataComponent BuiltinData
        // {
        //     get;
        //     private set;
        // }
        private static void InitCustomComponents()
        {
            //BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
        }
        
        public static void QuitApplication()
        {
#if UNITY_EDITOR

            UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");

#endif
            Application.Quit();
        }
    }
}