using System;
using UnityEngine;

namespace Game.Client
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameObjectPoolPreload")]
    public class GameObjectPoolPreload : ScriptableObject
    {
        public GameObjectPoolPreloadItem[] items;
    }

    [Serializable]
    public class GameObjectPoolPreloadItem
    {
        public string prefabPath;
        public int count;
    }
}