using System.Collections.Generic;
using Game.Core;
using Game.Math;

namespace Game.Gameplay
{

    public static class PhysicsUtils
    {
        private static IPhysicsUtility s_PhysicsUtility;

        public static void Initialize(IPhysicsUtility physicsUtility)
        {
            s_PhysicsUtility = physicsUtility;
        }

        #region PhysicsCheck

        public static bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
            ref float3 hitPosition)
        {
            return s_PhysicsUtility.SphereCheck(startTrace, endTrace, radius, traceFlag, ref hitPosition);
        }

        public static bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
            ref float3 hitPosition, ref float3 hitNormal)
        {
            return s_PhysicsUtility.SphereCheck(startTrace, endTrace, radius, traceFlag, ref hitPosition,
                ref hitNormal);
        }

        public static bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag,
            ref float3 hitPosition, ref float3 hitNormal)
        {
            return s_PhysicsUtility.SphereCheck(startTrace, direction, radius, distance, traceFlag, ref hitPosition,
                ref hitNormal);
        }

        public static bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance, int traceFlag,
            ref ImpactInfo impactInfo)
        {
            return s_PhysicsUtility.SphereCheck(startTrace, direction, radius, distance, traceFlag, ref impactInfo);
        }

        public static bool SingleLineCheck(float3 startTrace, float3 endTrace, int traceFlag, ref ImpactInfo impactInfo)
        {
            return s_PhysicsUtility.SingleLineCheck(startTrace, endTrace, traceFlag, ref impactInfo);
        }

        public static bool SingleLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
            ref ImpactInfo impactInfo, bool bDrawLine = false, DebugColor color = DebugColor.kColorGreen,
            float duration = 5.0f)
        {
            return s_PhysicsUtility.SingleLineCheck(startTrace, direction, distance, traceFlag, ref impactInfo,
                bDrawLine,
                color, duration);
        }

        public static bool MultiLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
            ref List<ImpactInfo> impactList, bool blockByStaticActor = false)
        {
            return s_PhysicsUtility.MultiLineCheck(startTrace, direction, distance, traceFlag, ref impactList,
                blockByStaticActor);
        }

        #endregion
    }
}