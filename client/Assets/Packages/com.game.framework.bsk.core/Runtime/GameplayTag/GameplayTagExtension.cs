

using UnityEditor;

namespace Game.Gameplay
{
    public static partial class GameplayTagExtension
    {
        [MenuItem("GameplayTag/Save")]
        public static void SaveFile() => GameplayTagHelper.SaveTagFile();
        
    }
}