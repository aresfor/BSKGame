
using UnityEngine;

namespace Game.Client
{
    public class TransformSocketMapName
    {
        public const string FPSkinnedMeshRendererSocket = "FPSkinnedMeshRendererSocket";        //FP ARM sknniedMeshRenderer所在的节点
        public const string TPDefaultWeaponSocket = "TPDefaultWeaponSocket";                    //TP武器挂载在角色身上的挂点
        public const string AnimatorRootBone = "AnimatorRootBone";                              //FP武器animator的rootbone节点
        public const string WeaponFireMuzzleFlashSocket = "WeaponFireMuzzleFlashSocket";        //武器开火 枪口特效挂点
        // public const string RightHandWeaponSocket = "RightHandWeapon";        //武器开火 枪口特效挂点
    }

    public class TransformSocketMap:MonoBehaviour
    {
        [SerializeField]
        public SerializableDictionary<string, Transform> m_SocketMap = new SerializableDictionary<string, Transform>();
    }
}