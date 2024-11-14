using Game.Core;
using UnityEngine;

namespace Game.Client
{
    public class MonoLattice:BaseMonoBehaviour
    {
        public void Initialize(Vector3 position, Quaternion rotation, Vector3 scale,
            Transform parent = null)
        {
            CachedTransform.position = position;
            CachedTransform.rotation = rotation;
            CachedTransform.localScale = scale;
            if(parent != null)
                CachedTransform.SetParent(parent, true);
        }
    }

    
}