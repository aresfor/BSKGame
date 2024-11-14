using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core
{
    public class GLDebugTool : MonoSingleton<GLDebugTool>
    {
        public Material DebugMaterial;

        struct Circle
        {
            public Vector3 startPos;
            public Vector3 endPos;
            public float radius;
            public Color color;
            public float time;
        }

        List<Circle> circleList = new List<Circle>();

        public void GLDrawCircle(Vector3 startPos, Vector3 endPos, float radius, Color color, float duration,
            bool depthTest = true)
        {
            Circle circle = new Circle();
            circle.startPos = startPos;
            circle.endPos = endPos;
            circle.radius = radius;
            circle.color = color;
            circle.time = Time.unscaledTime + duration;
            circleList.Add(circle);
        }

        private void DrawCircle(Circle circle)
        {
            if (null == DebugMaterial) return;
            DebugMaterial.SetPass(0);
            GL.wireframe = true;
            GL.Color(circle.color);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, (circle.endPos - circle.startPos));
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float r = 0; r < 360f; r += 10f)
            {
                Vector3 off = rot * Quaternion.AngleAxis(r, Vector3.down) * Vector3.right * circle.radius;

                if (r == 0)
                {
                    firstPoint = circle.endPos + off;
                }
                else
                {
                    beginPoint = circle.endPos + off;
                    GL.Vertex(firstPoint);
                    GL.Vertex(beginPoint);
                    firstPoint = beginPoint;
                }
            }

            GL.End();
            GL.PopMatrix();
            GL.wireframe = false;
        }

        private void DrawCircleList()
        {
            for (int i = 0; i < circleList.Count; i++)
            {
                if (circleList[i].time < Time.unscaledTime)
                {
                    circleList.Remove(circleList[i]);
                    i = i - 1;
                    continue;
                }

                DrawCircle(circleList[i]);
            }
        }

        void OnRenderObject()
        {
            DrawCircleList();
        }

        protected override void Dispose()
        {
        }
    }
}