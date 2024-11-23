
#if UNITY_EDITOR
using Game.Gameplay;
using GameFramework;
using UnityEditor;
namespace Game.Client
{
    public static partial class UnityGameplayTagExtension
    {
        [MenuItem("GameplayTag/Save")]
        public static void SaveTagFile()
        {
            GameplayTagHelper.SaveTagFile();
            AssetDatabase.Refresh();
        }

        public static GameplayTagTree InitializeGameplayTag(string json)
        {
            return GameplayTagHelper.TagTree = Utility.Json.ToObject<GameplayTagTree>(json);
        }
        
    }
}
#endif