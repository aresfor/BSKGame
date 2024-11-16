using Game.Math;

namespace Game.Core;

public static class PhysicsUtils
{
    private static IPhysicsUtility sPhysicsUtility;

    public static void Initialize(IPhysicsUtility physicsUtility)
    {
        sPhysicsUtility = physicsUtility;
    }
    
    #region PhysicsCheck

    public static bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
        ref float3 hitPosition)
    {
        return sPhysicsUtility.SphereCheck(startTrace, endTrace, radius, traceFlag, ref hitPosition);
    }

    public static bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
        ref float3 hitPosition, ref float3 hitNormal)
    {
        return sPhysicsUtility.SphereCheck(startTrace, endTrace, radius, traceFlag, ref hitPosition, ref hitNormal);
    }

    public static bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag,
        ref float3 hitPosition, ref float3 hitNormal)
    {
        return sPhysicsUtility.SphereCheck(startTrace, direction, radius, distance, traceFlag, ref hitPosition,
            ref hitNormal);
    }

    public static bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag,
        ref ImpactInfo impactInfo)
    {
        return sPhysicsUtility.SphereCheck(startTrace, direction, radius, distance, traceFlag, ref impactInfo);
    }

    public static bool SingleLineCheck(float3 startTrace, float3 endTrace, int traceFlag, ref ImpactInfo impactInfo)
    {
        return sPhysicsUtility.SingleLineCheck(startTrace, endTrace, traceFlag, ref impactInfo);
    }

    public static bool SingleLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
        ref ImpactInfo impactInfo, bool bDrawLine = false, DebugColor color = DebugColor.kColorGreen,
        float duration = 5.0f)
    {
        return sPhysicsUtility.SingleLineCheck(startTrace, direction, distance, traceFlag, ref impactInfo, bDrawLine,
            color, duration);
    }

    public static bool MultiLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
        ref List<ImpactInfo> impactList, bool blockByStaticActor = false)
    {
        return sPhysicsUtility.MultiLineCheck(startTrace, direction, distance, traceFlag, ref impactList,
            blockByStaticActor);
    }

    #endregion
}