using System;
using UnityEngine;

namespace Game.Core
{
    public interface IObjectPoolUtility : IUtility
    {
        public GameObject Spawn(string assetPath);
        public void SpawnAsync(string assetPath, Action<GameObject> callback);
        public void DeSpawn(GameObject unityGameObject);
        public void DeSpawn(GameObject unityGameObject, float delayTime);
    }
}