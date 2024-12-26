using UnityEngine;

namespace Game.Client
{
    public class SocketUtils
    {
        //查找Bone Socket
        public static Transform FindSocketTsFromTarget(string socketName, Transform parent,bool isActiveOnly = false)
        {
            if (string.IsNullOrEmpty(socketName) || parent == null)
            {
                return null;
            }
			
            Transform socketTrans = null;
            var childrenArray = parent.GetComponentsInChildren<Transform>(!isActiveOnly);
            foreach (var child in childrenArray)
            {
                if (child.name != socketName)
                    continue;

                socketTrans = child;
                break;
            }
            return socketTrans;
        }
    }
}