using UnityEngine;

using System.Diagnostics;

namespace Game.Core
{
    public static class DebugTool
    {
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, DebugColor color, float duration = 0.0f,
            bool depthTest = true)
        {
            var transferColor = DebugTool.TransferColor(color);
            UnityEngine.Debug.DrawLine(start, end, transferColor, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, UnityEngine.Color color, float duration = 0.0f,
            bool depthTest = true)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, UnityEngine.Color color, float duration = 0.0f,
            bool depthTest = true)
        {
            UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawCapsule(Vector3 start, Vector3 end, float radius, UnityEngine.Color color,
            float duration = 0.0f, bool depthTest = true)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, end - start);
            for (float r = 0; r < 360f; r += 30f)
            {
                Vector3 off = rot * Quaternion.AngleAxis(r, Vector3.down) * Vector3.right * radius;
                Vector3 offb = rot * Quaternion.AngleAxis(r + 30f, Vector3.down) * Vector3.right * radius;
                UnityEngine.Debug.DrawLine(start + off, end + off, color, duration, depthTest);
                UnityEngine.Debug.DrawLine(start + off, start + offb, color, duration, depthTest);
                UnityEngine.Debug.DrawLine(end + off, end + offb, color, duration, depthTest);
                for (float a = 0; a < 90f; a += 15f)
                {
                    Vector3 off2a = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(a, Vector3.back) *
                                    Vector3.right * radius;
                    Vector3 off2b = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(a + 15f, Vector3.back) * Vector3.right * radius;
                    UnityEngine.Debug.DrawLine(start + off2a, start + off2b, color, duration, depthTest);
                    Vector3 off3a = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(-a, Vector3.back) *
                                    Vector3.right * radius;
                    Vector3 off3b = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(-a - 15f, Vector3.back) * Vector3.right * radius;
                    UnityEngine.Debug.DrawLine(end + off3a, end + off3b, color, duration, depthTest);
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawGizmosCapsule(Vector3 start, Vector3 end, float radius)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, end - start);
            for (float r = 0; r < 360f; r += 30f)
            {
                Vector3 off = rot * Quaternion.AngleAxis(r, Vector3.down) * Vector3.right * radius;
                Vector3 offb = rot * Quaternion.AngleAxis(r + 30f, Vector3.down) * Vector3.right * radius;
                Gizmos.DrawLine(start + off, end + off);
                Gizmos.DrawLine(start + off, start + offb);
                Gizmos.DrawLine(end + off, end + offb);
                for (float a = 0; a < 90f; a += 15f)
                {
                    Vector3 off2a = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(a, Vector3.back) *
                                    Vector3.right * radius;
                    Vector3 off2b = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(a + 15f, Vector3.back) * Vector3.right * radius;
                    Gizmos.DrawLine(start + off2a, start + off2b);
                    Vector3 off3a = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(-a, Vector3.back) *
                                    Vector3.right * radius;
                    Vector3 off3b = rot * Quaternion.AngleAxis(r, Vector3.down) *
                                    Quaternion.AngleAxis(-a - 15f, Vector3.back) * Vector3.right * radius;
                    Gizmos.DrawLine(end + off3a, end + off3b);
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawCircle(Vector3 startPos, Vector3 endPos, float radius, Color color, float duration,
            bool depthTest = true)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, (endPos - startPos));
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float r = 0; r < 360f; r += 10f)
            {
                Vector3 off = rot * Quaternion.AngleAxis(r, Vector3.down) * Vector3.right * radius;

                if (r == 0)
                {
                    firstPoint = endPos + off;
                }
                else
                {
                    beginPoint = endPos + off;
                    UnityEngine.Debug.DrawLine(firstPoint, beginPoint, color, duration, depthTest);
                    firstPoint = beginPoint;
                }
            }
        }

        private struct DebugBox
        {
            public Vector3 localFrontTopLeft { get; private set; }
            public Vector3 localFrontTopRight { get; private set; }
            public Vector3 localFrontBottomLeft { get; private set; }
            public Vector3 localFrontBottomRight { get; private set; }

            public Vector3 localBackTopLeft
            {
                get { return -localFrontBottomRight; }
            }

            public Vector3 localBackTopRight
            {
                get { return -localFrontBottomLeft; }
            }

            public Vector3 localBackBottomLeft
            {
                get { return -localFrontTopRight; }
            }

            public Vector3 localBackBottomRight
            {
                get { return -localFrontTopLeft; }
            }

            public Vector3 frontTopLeft
            {
                get { return localFrontTopLeft + origin; }
            }

            public Vector3 frontTopRight
            {
                get { return localFrontTopRight + origin; }
            }

            public Vector3 frontBottomLeft
            {
                get { return localFrontBottomLeft + origin; }
            }

            public Vector3 frontBottomRight
            {
                get { return localFrontBottomRight + origin; }
            }

            public Vector3 backTopLeft
            {
                get { return localBackTopLeft + origin; }
            }

            public Vector3 backTopRight
            {
                get { return localBackTopRight + origin; }
            }

            public Vector3 backBottomLeft
            {
                get { return localBackBottomLeft + origin; }
            }

            public Vector3 backBottomRight
            {
                get { return localBackBottomRight + origin; }
            }

            public Vector3 origin { get; private set; }

            public DebugBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
            {
                Rotate(orientation);
            }

            public DebugBox(Vector3 origin, Vector3 halfExtents)
            {
                this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

                this.origin = origin;
            }

            Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
            {
                Vector3 direction = point - pivot;
                return pivot + rotation * direction;
            }

            public void Rotate(Quaternion orientation)
            {
                localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
                localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
                localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
                localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
            }
        }


        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color,
            float duration = -1.0f)
        {
#if UNITY_EDITOR
            DebugBox box = new DebugBox(origin, halfExtents, orientation);
            UnityEngine.Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color, duration);

            UnityEngine.Debug.DrawLine(box.backTopLeft, box.backTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.backTopRight, box.backBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color, duration);

            UnityEngine.Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.frontTopRight, box.backTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color, duration);
#endif
        }

        
        #region Gizmos

        public static void DrawGizmosSphere(Vector3 center, float radius,Color color,float lifetimer)
        {
            DrawGizmos.Instance.DrawSphereGizmos(center.ToFloat3(),radius,color,lifetimer);
        }
        #endregion
        public static UnityEngine.Color TransferColor(DebugColor color)
        {
            switch (color)
            {
                case DebugColor.kColorcyan:
                    return Color.cyan;
                case DebugColor.kColorGrey:
                    return Color.grey;
                case DebugColor.kColorGray:
                    return Color.gray;
                case DebugColor.kColorMagenta:
                    return Color.magenta;
                case DebugColor.kColorRed:
                    return Color.red;
                case DebugColor.kColorYellow:
                    return Color.yellow;
                case DebugColor.kColorBlack:
                    return Color.black;
                case DebugColor.kColorWhite:
                    return Color.white;
                case DebugColor.kColorGreen:
                    return Color.green;
                case DebugColor.kColorBlue:
                    return Color.blue;
            }

            return Color.blue;
        }
    }
}