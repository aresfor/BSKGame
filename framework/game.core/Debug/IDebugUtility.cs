using Game.Math;

namespace Game.Core;

public interface IDebugUtility : IUtility
{
    void BeginProfiler(string name);

    void EndProfiler();

    void DrawLine(float3 start, float3 end, DebugColor color, float duration = 0.0f, bool depthTest = true);

    void DrawRay(float3 start, float3 dir, DebugColor color, float duration = 0.0f, bool depthTest = true);

    void DrawCapsule(
        float3 start,
        float3 end,
        float radius,
        DebugColor color,
        float duration = 0.0f,
        bool depthTest = true);

    void DrawGizmosCapsule(float3 start, float3 end, float radius);

    void DrawBox(
        float3 origin,
        float3 halfExtents,
        quaternion orientation,
        DebugColor color,
        float duration = -1f);

    void DrawCircle(
        float3 start,
        float3 end,
        float radius,
        DebugColor color,
        float duration = 0.0f,
        bool depthTest = true);

    void GLDrawCircle(
        float3 start,
        float3 end,
        float radius,
        DebugColor color,
        float duration = 0.0f,
        bool depthTest = true);

    void DrawSphereGizmos(float3 center, float raidus, DebugColor color, float duration = 0.0f);
}