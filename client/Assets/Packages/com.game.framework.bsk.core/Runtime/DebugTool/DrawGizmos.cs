using System.Collections.Generic;
using UnityEngine;
using Game.Math;

namespace Game.Core
{
    public enum EScanType
    {
        Circle,
        SectorCircle,
        Rect,
        Annular,
    }
    public class DrawGizmos : MonoSingleton<DrawGizmos>
    {
        private float drawTime = 0f;

        Vector3 position;
        Vector3 forward;

        Mesh tempMesh;
        Mesh tempMesh2;

        class SphereGizmosInfo
        {
            public SphereGizmosInfo(float3 _center, float _radius, Color _color, float _lifeTimer)
            {
                center = _center;
                radius = _radius;
                color = _color;
                lifeTimer = _lifeTimer;
            }
            public float3 center;
            public Color color;
            public float radius;
            public float lifeTimer;
            public float elapseTime;
        }

        private List<SphereGizmosInfo> sphereGizemos = new ();

        private void OnDrawGizmos()
        {
            if (Time.time < drawTime)
            {
                transform.parent.position = position;
                transform.parent.forward = forward;
                if (tempMesh != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawMesh(tempMesh, position + Vector3.up * 0.01f, transform.rotation);
                }
                if (tempMesh2 != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawMesh(tempMesh2, position + Vector3.up * 0.01f, transform.rotation);
                }
            }
            
            //draw sphere gizmos
            for (int i = sphereGizemos.Count - 1; i >= 0; i--)
            {
                Gizmos.color = sphereGizemos[i].color;
                Gizmos.DrawSphere(sphereGizemos[i].center.ToVector3(),sphereGizemos[i].radius);
                sphereGizemos[i].elapseTime += Time.deltaTime;
                if (sphereGizemos[i].elapseTime > sphereGizemos[i].lifeTimer)
                {
                    sphereGizemos.RemoveAt(i);
                }
            }
        }

        public void Draw(Vector3 position, Vector3 forward, EScanType scanType, List<int> scanParam)
        {
    #if UNITY_EDITOR
            ClearMesh();
            this.position = position;
            this.forward = forward;
            drawTime = Time.time + 1f;
            transform.localRotation = Quaternion.identity;
            switch (scanType)
            {
                case EScanType.Circle:
                    tempMesh = CreateMesh(scanParam[0] * 0.01f, 0, 360);
                    tempMesh.RecalculateNormals();
                    break;
                case EScanType.SectorCircle:
                    float angle = scanParam[2];
                    transform.localRotation = Quaternion.Euler(0, angle / 2 - 90, 0);
                    tempMesh = CreateMesh(scanParam[0] * 0.01f, 0, angle);
                    tempMesh.RecalculateNormals();
                    if (scanParam[1] > 0)
                    {
                        tempMesh2 = CreateMesh(scanParam[1] * 0.01f, 0, angle);
                        tempMesh2.RecalculateNormals();
                    }
                    break;
                case EScanType.Rect:
                    var height = scanParam[0] * 0.01f;
                    var width = scanParam[1] * 0.01f;
                    tempMesh = CreateRectMesh(width, height);
                    tempMesh.RecalculateNormals();
                    break;
                case EScanType.Annular:
                    break;
            }
    #endif
        }
        /// <summary>
        /// 创建扇形网格
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="innerradius">内半径</param>
        /// <param name="angledegree">扇形角度</param>
        /// <param name="segments">精准度</param>
        /// <returns></returns>
        public Mesh CreateMesh(float radius, float innerradius, float angledegree, int segments = 60)
        {
            //vertices(顶点):
            int vertices_count = segments * 2 + 2;              //因为vertices(顶点)的个数与triangles（索引三角形顶点数）必须匹配
            Vector3[] vertices = new Vector3[vertices_count];
            float angleRad = Mathf.Deg2Rad * angledegree;
            float angleCur = angleRad;
            float angledelta = angleRad / segments;
            for (int i = 0; i < vertices_count; i += 2)
            {
                float cosA = Mathf.Cos(angleCur);
                float sinA = Mathf.Sin(angleCur);

                vertices[i] = new Vector3(radius * cosA, 0, radius * sinA);
                vertices[i + 1] = new Vector3(innerradius * cosA, 0, innerradius * sinA);
                angleCur -= angledelta;
            }

            //triangles:
            int triangle_count = segments * 6;
            int[] triangles = new int[triangle_count];
            for (int i = 0, vi = 0; i < triangle_count; i += 6, vi += 2)
            {
                triangles[i] = vi;
                triangles[i + 1] = vi + 3;
                triangles[i + 2] = vi + 1;
                triangles[i + 3] = vi + 2;
                triangles[i + 4] = vi + 3;
                triangles[i + 5] = vi;
            }

            //uv:
            Vector2[] uvs = new Vector2[vertices_count];
            for (int i = 0; i < vertices_count; i++)
            {
                uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
            }

            //负载属性与mesh
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            return mesh;
        }

        public Mesh CreateRectMesh(float width, float height)
        {
            float halfWidth = width / 2;
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>()
            {
                new Vector3(-halfWidth, 0, 0),
                new Vector3(-halfWidth, 0, height),
                new Vector3(halfWidth, 0, height),
                new Vector3(halfWidth, 0, 0)
            };
            List<int> ints = new List<int>() { 0, 1, 2, 0, 2, 3 };
            mesh.vertices = vertices.ToArray();
            mesh.triangles = ints.ToArray();
            return mesh;
        }

        public void ClearMesh()
        {
            if (tempMesh != null)
            {
                Destroy(tempMesh);
                tempMesh = null;
            }
            if (tempMesh2 != null)
            {
                Destroy(tempMesh2);
                tempMesh2 = null;
            }
        }

        public void DrawSphereGizmos(float3 center, float raidus, Color color, float duration = 0)
        {
            sphereGizemos.Add(new SphereGizmosInfo(center,raidus,color,duration));
        }

        protected override void Dispose()
        {
            ClearMesh();
        }

        private void OnDestroy()
        {
            ClearMesh();
        }
    }

}
