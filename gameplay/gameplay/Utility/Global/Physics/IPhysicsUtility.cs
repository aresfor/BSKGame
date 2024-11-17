using Game.Core;
using Game.Math;

namespace Game.Gameplay;

public interface IPhysicsUtility: IUtility
{
    #region PhysicsCheck

    bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,ref float3 hitPosition);
    bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag, ref float3 hitPosition, ref float3 hitNormal);
    bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag, ref float3 hitPosition, ref float3 hitNormal);
    bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag, ref ImpactInfo impactInfo);
    bool SingleLineCheck(float3 startTrace, float3 endTrace, int traceFlag, ref ImpactInfo impactInfo); 
    bool SingleLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag, ref ImpactInfo impactInfo,  bool bDrawLine = false, DebugColor color = DebugColor.kColorGreen, float duration=5.0f);
    bool MultiLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag, ref List<ImpactInfo> impactList, bool blockByStaticActor = false);

    #endregion

    #region PhysicsSwap NonAlloc (暂时保留，考虑移除)
    int OverlapSphereNonAlloc(float3 startPos, float radius, int traceFlag, ref List<ImpactInfo> impactList,int maxCount);
    int BoxCastNonAlloc(float3 startPos,float3 halfExtents,float3 direction, quaternion orientation,float maxDistance,int traceFlag, ref List<ImpactInfo> impactList,int maxCount,
    bool bIsIgnoreDetailCheck = false);
    #endregion
    
    #region NoPhysics
    
    bool SingleLineCheckNoPhysics(uint pawnEntityID, float3 startTrace, float3 direction, float distance, int obstacleFlag, ref ImpactInfo impactInfo);
    bool SingleLineCheckNoPhysics(uint pawnEntityID, float3 startTrace, float3 direction, float distance, int obstacleFlag, ref ImpactInfo impactInfo,  bool bDrawLine = false, DebugColor color = DebugColor.kColorGray, float duration=5.0f);

    #endregion
}




public class ImpactInfo
{
    public uint HitEntityId = 0;//可以为任意注册了的类型
    public float Damage;
    public float3 HitObjPosition;   //击中点 object的位置
    public float3 HitLocation;      //击中点 world position
    public float3 HitNormal;        //击中点 法线
    public float3 RayDir;           //检测射线方向
    public float3 StartPosition;    //检测射线起始点             
    public float Distance;          //击中点到起始点距离，映射unity RaycastHit.distance
    public LayerType ActorLayer;          //击中collider的layer
    public int ThroughWallCount;    //穿墙次数
    public int ThroughPlayerCount;  //穿人次数
    public EHitGroup HitGroup;      //击中部位
    public int InstanceId;          // 客户端对应的是collider.GetInstanceId()，服务器对应的是ActorId
    public uint materialId;         // 配置表中的材质id 击中的collider 物理材质ID
	public bool bPeneOut;           // 是否穿出点
    public int HitTypeMask;
    public uint ImpactTag;//用于分辨特殊ImpactInfo的Tag
    
    
    public static void Copy(ImpactInfo source,ImpactInfo target)
    {
        target.HitEntityId = source.HitEntityId;
        target.Damage = source.Damage;
        target.HitObjPosition = source.HitObjPosition;
        target.HitNormal = source.HitNormal;
        target.RayDir = source.RayDir;
        target.StartPosition = source.StartPosition;
        target.Distance = source.Distance;
        target.ActorLayer = source.ActorLayer;
        target.ThroughWallCount = source.ThroughWallCount;
        target.ThroughPlayerCount = source.ThroughPlayerCount;
        target.HitGroup = source.HitGroup;
        target.InstanceId = source.InstanceId;
        target.materialId = source.materialId;
        target.bPeneOut = source.bPeneOut;
        target.HitTypeMask = source.HitTypeMask;
        target.ImpactTag = source.ImpactTag;
    }

    #region Rold

    public uint HitRoleEntityId
    {
        get
        {
            // EEntityArchetype.PlayerPawn
            return HitEntityId;//Check 0?
        }
    }

    #endregion

    public const uint AutoAimCollider = 0;
    public const uint AutoFireCollider = 1;

	protected static List<ImpactInfo> m_CacheList = new List<ImpactInfo>();

    public static void Init(int count)
    {
        m_CacheList.Clear();
        for (int i = 0; i < count; ++i)
        {
            m_CacheList.Add(new ImpactInfo());
        }
    }

    public static void ShutDown()
    {
        m_CacheList.Clear();
    }

    public void OnRecycle()
    {
        ThroughWallCount = 0;
        ThroughPlayerCount = 0;
        Distance = 0;
        HitGroup = EHitGroup.Default;
        ActorLayer = LayerType.kDefault;
        HitEntityId = 0;
        materialId = 0;
        bPeneOut = false;
        ImpactTag = 0;
        HitTypeMask = 0;
    }

    public static ImpactInfo Alloc()
    {
        if (m_CacheList.Count > 0)
        {
            ImpactInfo info = m_CacheList[m_CacheList.Count - 1];
            m_CacheList.RemoveAt(m_CacheList.Count - 1);
            return info;
        }
        return new ImpactInfo();
    }

    public static void Recycle(ImpactInfo info)
    {
        if (info != null && !m_CacheList.Contains(info))
        {
            info.OnRecycle();
            m_CacheList.Add(info);
        }
    }

    public static void RecycleImpactInfo(List<ImpactInfo> ImpactList)
    {
        if (ImpactList != null)
        {
            for (int i = 0; i < ImpactList.Count; i++)
            {
                Recycle(ImpactList[i]);
            }
            ImpactList.Clear();
        }
    }
}

public struct NotPhyHitResult
{
    public float3 HitLocation;
    public float3 HitNormal;
    public float Distance;
    
    public void Reset()
    {
        HitLocation = float3.zero;
        HitNormal = float3.zero;
        Distance = 0;
    }
}