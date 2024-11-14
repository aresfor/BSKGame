using UnityEngine;
using Game.Core;
using Game.Math;
using UnityEngine.Profiling;

//using CustomGamePlay;


namespace Game.Core
{
     public class UnityDebugToolService : IDebugUtility
    {

        UnityEngine.Color TransferColor(DebugColor color)
        {
            return DebugTool.TransferColor(color);
        }


        public void DrawBox(float3 origin, float3 halfExtents, quaternion orientation, DebugColor color, float duration = -1)
        {
            DebugTool.DrawBox(origin.ToVector3(), halfExtents.ToVector3(), orientation.ToQuaternion(), TransferColor(color),duration);
        }

        public void DrawCircle(float3 start, float3 end, float radius, DebugColor color, float duration = 0, bool depthTest = true)
        {
            DebugTool.DrawCircle(start.ToVector3(), end.ToVector3(), radius, TransferColor(color), duration, depthTest);
        }

        public void GLDrawCircle(float3 start, float3 end, float radius, DebugColor color, float duration = 0, bool depthTest = true)
        {
            GLDebugTool.Instance.GLDrawCircle(start.ToVector3(), end.ToVector3(), radius, TransferColor(color), duration, depthTest);
        }

        public void DrawSphereGizmos(float3 center, float raidus, DebugColor color, float duration = 0)
        {
            DrawGizmos.Instance.DrawSphereGizmos(center,raidus,TransferColor(color),duration);
        }

        public void DrawCapsule(float3 start, float3 end, float radius, DebugColor color, float duration = 0, bool depthTest = true)
        {
            DebugTool.DrawCapsule(start.ToVector3(), end.ToVector3(), radius, TransferColor(color), duration, depthTest);
        }

        public void DrawGizmosCapsule(float3 start, float3 end, float radius)
        {
            DebugTool.DrawGizmosCapsule(start.ToVector3(), end.ToVector3(), radius);
        }

        public void DrawLine(float3 start, float3 end, DebugColor color, float duration = 0, bool depthTest = true)
        {
            DebugTool.DrawLine(start.ToVector3(), end.ToVector3(),  TransferColor(color), duration, depthTest);
        }

        public void DrawRay(float3 start, float3 dir, DebugColor color, float duration = 0, bool depthTest = true)
        {
            //float3 endpos = start + dir * 100;
            //DebugTool.DrawLine(start.ToVector3(), endpos.ToVector3(), TransferColor(color), duration, depthTest);
            DebugTool.DrawRay(start.ToVector3(), dir.ToVector3(), TransferColor(color), duration, depthTest);
        }
        

        public void BeginProfiler(string name)
        {
            Profiler.BeginSample(name);
        }

        public void EndProfiler()
        {
            Profiler.EndSample();
        }
    }
}