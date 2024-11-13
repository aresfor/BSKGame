using UnityEngine;

namespace Game.Client
{
    public class MonoLattice:CacheMonoBehaviour
    {
        public void Initialize(Vector3 position, Quaternion rotation, Vector3 scale,
            Transform parent = null)
        {
            CacheTransform.position = position;
            CacheTransform.rotation = rotation;
            CacheTransform.localScale = scale;
            if(parent != null)
                CacheTransform.SetParent(parent, true);
        }
    }

    
}