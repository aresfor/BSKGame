using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class ColliderBindLogicSocket:EntityLogicSocketBase
    {
        private Dictionary<string, Collider> m_Colliders = new Dictionary<string, Collider>();
        public Transform ModelRoot;

        public void CollectAllCollider()
        {
            CollectCollider(ModelRoot);
            RecursiveChildTransform(ModelRoot, CollectCollider);
        }
        
        private void CollectCollider(Transform ts)
        {
            var collider = ts.GetComponent<Collider>();
            if (collider != null)
            {
                if (m_Colliders.ContainsKey(collider.name))
                {
                    Log.Error("重名Collider");
                    return;
                }

                m_Colliders[collider.name] = collider;
                collider.GetOrAddComponent<EntityColliderBinder>().Entity = Entity;

            }
        }

        public void RecursiveChildTransform(Transform ts, Action<Transform> action)
        {
            if (ts != null)
            {
                foreach (Transform childTransform in ts.transform)
                {
                    action.Invoke(childTransform);
                    RecursiveChildTransform(childTransform, action);
                }
            }
        }
    }
}