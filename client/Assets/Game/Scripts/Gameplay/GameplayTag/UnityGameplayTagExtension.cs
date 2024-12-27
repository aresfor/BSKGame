
using Game.Core;
using Game.Gameplay;
using GameFramework;
#if UNITY_EDITOR

using UnityEditor;
#endif

namespace Game.Client
{
    //@TODO：需要一个类似UE的gameplaytag编辑器
    public static partial class UnityGameplayTagExtension
    {
#if UNITY_EDITOR
        [MenuItem("GameplayTag/Save")]
        public static void SaveTagFile()
        {
            GameplayTagHelper.SaveTagFile();
            AssetDatabase.Refresh();
        }
#endif

        public static GameplayTagTree InitializeGameplayTag(string json)
        {
            return GameplayTagHelper.TagTree = Utility.Json.ToObject<GameplayTagTree>(json);
        }
        
    }
}
