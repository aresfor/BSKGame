
using Game.Core;
using Game.Math;
using UnityEngine;

namespace Game.Client
{
    public static class NoPhysicUtil
    {
        public static float Square2D(float3 v)
        {
            return v.x * v.x + v.y * v.y;
        }
        public static  bool RaycastAABB(float3 _center, float3 _extent, float3 _start, float3 _end, out float hitDis)
        {
            Vector3 center = _center.ToVector3();
            Vector3 extent = _extent.ToVector3();
            Vector3 start = _start.ToVector3();
            Vector3 end = _end.ToVector3();
            Vector3 boxMinCorner = center - extent;
            Vector3 boxMaxCorner = center + extent;

            hitDis = 0;

            bool insideBox = true;
            Vector3 dir = end - start;
            Vector3 hitTime = Vector3.zero;

            if (start.x < boxMinCorner.x)
            {
                if (dir.x <= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.x = (boxMinCorner.x - start.x) / dir.x;
                }
            }
            else if (start.x > boxMaxCorner.x)
            {
                if (dir.x >= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.x = (boxMaxCorner.x - start.x) / dir.x;
                }
            }
            else
            {
                hitTime.x = 0;
            }

            if (hitTime.x >= 1)
            {
                return false;
            }

            if (start.y < boxMinCorner.y)
            {
                if (dir.y <= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.y = (boxMinCorner.y - start.y) / dir.y;
                }
            }
            else if (start.y > boxMaxCorner.y)
            {
                if (dir.y >= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.y = (boxMaxCorner.y - start.y) / dir.y;
                }
            }
            else
            {
                hitTime.y = 0;
            }

            if (hitTime.y >= 1)
            {
                return false;
            }

            if (start.z < boxMinCorner.z)
            {
                if (dir.z <= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.z = (boxMinCorner.z - start.z) / dir.z;
                }
            }
            else if (start.z > boxMaxCorner.z)
            {
                if (dir.z >= 0)
                {
                    return false;
                }
                else
                {
                    insideBox = false;
                    hitTime.z = (boxMaxCorner.z - start.z) / dir.z;
                }
            }
            else
            {
                hitTime.z = 0;
            }

            if (hitTime.z >= 1)
            {
                return false;
            }

            if (insideBox)
            {
                hitDis = float.Epsilon;
                return true;
            }
            else
            {
                float realHitTime = hitTime.x;
                realHitTime = hitTime.y > realHitTime ? hitTime.y : realHitTime;
                realHitTime = hitTime.z > realHitTime ? hitTime.z : realHitTime;
                float epsilon = 0.00001f;
                if (realHitTime > 0)
                {
                    Vector3 hitPos = start + dir * realHitTime;
                    if (hitPos.x >= boxMinCorner.x - epsilon && hitPos.x <= boxMaxCorner.x + epsilon &&
                        hitPos.y >= boxMinCorner.y - epsilon && hitPos.y <= boxMaxCorner.y + epsilon &&
                        hitPos.z >= boxMinCorner.z - epsilon && hitPos.z <= boxMaxCorner.z + epsilon)
                    {
                        hitDis = Vector3.Distance(start, hitPos);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}