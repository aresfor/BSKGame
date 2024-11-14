using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game.Math;


namespace Game.Core
{
    public static class MathAdapter
    {
        public static Vector3 ToUnityVector3(this float3 vector3)
        {
            var tempVec3 = Vector3.zero;
            tempVec3.x = vector3.x;
            tempVec3.y = vector3.y;
            tempVec3.z = vector3.z;
            return tempVec3;
        }

        public static float2 ToFloat2(this Vector2 v)
        {
            return new float2(v.x, v.y);
        }

        public static Vector2 ToVector2(this float2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static float3 ToFloat3(this Vector3 v)
        {
            return new float3(v.x, v.y, v.z);
        }

        public static Vector3 ToVector3(this float3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static float4 ToFloat4(this Vector4 v)
        {
            return new float4(v.x, v.y, v.z, v.w);
        }

        public static Vector4 ToVector4(this float4 v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static Quaternion ToQuaternion(this float3 euler)
        {
            return Quaternion.Euler(euler.x, euler.y, euler.z);
        }
        
        public static Quaternion ToQuaternion(this quaternion q)
        {
            return new Quaternion(q.value.x, q.value.y, q.value.z, q.value.w);
        }

        public static quaternion ToQuaternion(this Quaternion q)
        {
            return new quaternion(q.x, q.y, q.z, q.w);
        }

        public static Matrix4x4 ToMatrix4x4(this float4x4 m)
        {
            return new Matrix4x4(m.c0.ToVector4(), m.c1.ToVector4(), m.c2.ToVector4(), m.c3.ToVector4());
        }

        public static float4x4 ToFloat4x4(this Matrix4x4 m)
        {
            return new float4x4(m.GetColumn(0).ToFloat4(), m.GetColumn(1).ToFloat4(), m.GetColumn(2).ToFloat4(), m.GetColumn(3).ToFloat4());
        }

        public static float Abs(float a)
        {
            return a < 0 ? -a : a;
        }
    }
}