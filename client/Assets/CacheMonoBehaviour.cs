
using UnityEngine;

namespace Game.Client
{
    public class CacheMonoBehaviour:MonoBehaviour
    {
        private Transform mTransfrom;

        public Transform CacheTransform
        {
            get
            {
                if (null == mTransfrom)
                    mTransfrom = transform;

                return mTransfrom;
            }
        }
    }
}
