
#if UNITY_EDITOR
using Game.Client;
using GameFramework;
using UnityEditor;

namespace Game.Gameplay
{
    public static partial class GameplayTagExtension
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