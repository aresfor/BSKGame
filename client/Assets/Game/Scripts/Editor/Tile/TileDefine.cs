
#if UNITY_EDITOR
using UnityEditor.Tilemaps;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Client.Editor
{
    public static class TileDefine
    {
        [CreateTileFromPalette]
        public static TileBase CustomTile(Sprite sprite)
        {
            var customTile = ScriptableObject.CreateInstance<CustomTile>();
            customTile.sprite = sprite;
            customTile.name = sprite.name;
            return customTile;
        }
    }
}
#endif