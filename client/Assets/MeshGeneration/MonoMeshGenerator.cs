using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering;
using static Unity.Mathematics.math;
using Game.Client;
using Unity.Collections.LowLevel.Unsafe;


namespace Game.Client
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MonoMeshGenerator:MonoBehaviour
    {
        private Mesh mMesh;
        
        private const int MeshCount = 1;
        [SerializeField, Range(1,10)]
        private int Resolution = 2;

        // private void OnValidate()
        // {
        //     if (mMesh != null)
        //     {
        //         Generate();
        //     }
        // }

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            mMesh = new Mesh()
            {
                name = "My Procedural Mesh",
            };
            
            int vertexAttributeCount = 4;
            int vertexCount = 4;
            int triangleIndexCount = 6;

            var meshDataArray = Mesh.AllocateWritableMeshData(MeshCount);
            Mesh.MeshData meshData = meshDataArray[0];
            
            // //如果手动初始化所有流，则不需要初始化内存
            // var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            //     vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            // );
            //
            // //多流
            // //一个网格区域最多支持四个流
            // // vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);
            // //
            // // vertexAttributes[1] = new VertexAttributeDescriptor(
            // //     VertexAttribute.Normal, dimension: 3, stream: 1
            // // );
            // // //降低切线和法线精度减少内存消耗，需要注意的是单个属性大小必须为4字节的倍数
            // // //因此像位置和法线无法使用half这种优化，不过可以使用其他数据格式
            // // vertexAttributes[2] = new VertexAttributeDescriptor(
            // //     VertexAttribute.Tangent, VertexAttributeFormat.Float16, dimension: 4, stream: 2
            // // );
            // // vertexAttributes[3] = new VertexAttributeDescriptor(
            // //     VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, dimension: 2, stream: 3
            // // );
            //
            // //单流
            // vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3, stream: 0);
            // vertexAttributes[1] = new VertexAttributeDescriptor(
            //     VertexAttribute.Normal, dimension: 3, stream: 0
            // );
            // vertexAttributes[2] = new VertexAttributeDescriptor(
            //     VertexAttribute.Tangent, VertexAttributeFormat.Float16, dimension: 4, stream: 0
            // );
            // vertexAttributes[3] = new VertexAttributeDescriptor(
            //     VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, dimension: 2, stream:0
            // );
            //
            //
            // //关联顶点属性和网格数据
            // meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
            // vertexAttributes.Dispose();
            //
            //
            // half h0 = half(0f), h1 = half(1f);
            // //多流
            // // NativeArray<float3> positions = meshData.GetVertexData<float3>();
            // // positions[0] = 0f;
            // // positions[1] = right();
            // // positions[2] = up();
            // // positions[3] = float3(1f, 1f, 0f);
            // //
            // // NativeArray<float3> normals = meshData.GetVertexData<float3>(1);
            // // normals[0] = normals[1] = normals[2] = normals[3] = back();
            // //
            // // NativeArray<half4> tangents = meshData.GetVertexData<half4>(2);
            // // tangents[0] = tangents[1] = tangents[2] = tangents[3] = half4(h1, h0, h0, half(-1));
            // //
            // // NativeArray<half2> texCoords = meshData.GetVertexData<half2>(3);
            // // texCoords[0] = half2(h0);
            // // texCoords[1] = half2(h1, h0);
            // // texCoords[2] = half2(h0, h1);
            // // texCoords[3] = half2(h1);
            //
            // //单流
            // NativeArray<Vertex> vertices = meshData.GetVertexData<Vertex>();
            // var vertex = new Vertex {
            //     normal = back(),
            //     tangent = half4(h1, h0, h0, half(-1f))
            // };
            //
            // vertex.position = 0f;
            // vertex.texCoord0 = h0;
            // vertices[0] = vertex;
            //
            // vertex.position = right();
            // vertex.texCoord0 = half2(h1, h0);
            // vertices[1] = vertex;
            //
            // vertex.position = up();
            // vertex.texCoord0 = half2(h0, h1);
            // vertices[2] = vertex;
            //
            // vertex.position = float3(1f, 1f, 0f);
            // vertex.texCoord0 = h1;
            // vertices[3] = vertex;
            //
            // //设置索引
            // //uint16最多支持65535，获取数据时对应托管类型ushort
            // meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt16);
            // NativeArray<ushort> triangleIndices = meshData.GetIndexData<ushort>();
            // triangleIndices[0] = 0;
            // triangleIndices[1] = 2;
            // triangleIndices[2] = 1;
            // triangleIndices[3] = 1;
            // triangleIndices[4] = 2;
            // triangleIndices[5] = 3;
            //
            // //应用网格数据到网格
            // meshData.subMeshCount = MeshCount;
            // var bounds = meshBounds;
            // //手动传递子网格边界，防止unity可能的不正确计算
            // meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount)
            // {
            //     bounds = bounds,
            //     vertexCount = vertexCount
            // }, MeshUpdateFlags.DontRecalculateBounds);
            
            MeshJob<SquareGridGenerator, SingleStream>.ScheduleParallel(mMesh , meshData, Resolution, default).Complete();
            
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mMesh);
            
            GetComponent<MeshFilter>().mesh = mMesh;
        }

    }
    
    //单流
    public struct Vertex {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
    
    //防止编译器更改内存布局，因为单流要求数据存储按照我们定义的精确
    [StructLayout(LayoutKind.Sequential)]
    public struct Stream0 {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
    
    


    public interface IMeshStreams
    {
        void Setup(Mesh.MeshData meshData, Bounds subMeshBounds, int vertexCount, int indexCount);
        void SetVertex(int index, Vertex data);
        void SetTriangle(int index, int3 triangle);
    }

    public struct MultiStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float3> mStream0, mStream1;
        //tangent
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float4> mStream2;
        //texCoord0
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float2> mStream3;
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> mTriangles;
        public void Setup(Mesh.MeshData meshData, Bounds subMeshBounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(
                4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );
            descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(
                VertexAttribute.Normal, dimension: 3, stream:1
            );
            descriptor[2] = new VertexAttributeDescriptor(
                VertexAttribute.Tangent, dimension: 4, stream:2
            );
            descriptor[3] = new VertexAttributeDescriptor(
                VertexAttribute.TexCoord0, dimension: 2, stream:3
            );
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16/*UInt32*/);
			
            meshData.subMeshCount = 1;
            //使用SetSubMesh时会立刻验证三角形索引和重新计算Bounds
            //但并不希望这样，因此设置Flags
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
                {
                    bounds = subMeshBounds,
                    vertexCount = vertexCount
                }
                , MeshUpdateFlags.DontRecalculateBounds |
                  MeshUpdateFlags.DontValidateIndices);
            
            mStream0 = meshData.GetVertexData<float3>(0);
            mStream1 = meshData.GetVertexData<float3>(1);
            mStream2 = meshData.GetVertexData<float4>(2);
            mStream3 = meshData.GetVertexData<float2>(3);
            
            mTriangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(sizeof(ushort));
        }

        public void SetVertex(int index, Vertex data)
        {
            mStream0[index] = data.position;
            mStream1[index] = data.normal;
            mStream2[index] = data.tangent;
            mStream3[index] = data.texCoord0;
        }

        public void SetTriangle(int index, int3 triangle)
        {
            mTriangles[index] = triangle;
        }
    }
    
    public struct SingleStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Stream0> mStream0;
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> mTriangles;
        public void Setup(Mesh.MeshData meshData, Bounds subMeshBounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(
                4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );
            descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(
                VertexAttribute.Normal, dimension: 3
            );
            descriptor[2] = new VertexAttributeDescriptor(
                VertexAttribute.Tangent, dimension: 4
            );
            descriptor[3] = new VertexAttributeDescriptor(
                VertexAttribute.TexCoord0, dimension: 2
            );
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16/*UInt32*/);
			
            meshData.subMeshCount = 1;
            //使用SetSubMesh时会立刻验证三角形索引和重新计算Bounds
            //但并不希望这样，因此设置Flags
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
                {
                    bounds = subMeshBounds,
                    vertexCount = vertexCount
                }
                , MeshUpdateFlags.DontRecalculateBounds |
                  MeshUpdateFlags.DontValidateIndices);
            
            mStream0 = meshData.GetVertexData<Stream0>();
            mTriangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(sizeof(ushort));
        }

        //向Burst指示内联编译方法，减少调用速度
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex data)
        {
            mStream0[index] = new Stream0()
            {
                position = data.position,
                normal = data.normal,
                tangent = data.tangent,
                texCoord0 = data.texCoord0
            };
        }

        public void SetTriangle(int index, int3 triangle)
        {
            mTriangles[index] = triangle;
        }
    }
    
    public interface IMeshGenerator {

        void Execute<S> (int i, S streams) where S : struct, IMeshStreams;
        int VertexCount { get=>4 *ResolutionSquare; }
		
        int IndexCount { get=>6 * ResolutionSquare; }
        int JobLength { get=>ResolutionSquare; }
        Bounds Bounds { get; }
        int Resolution { get; set; }
        int ResolutionSquare => Resolution * Resolution;
    }
    public struct SquareGridGenerator : IMeshGenerator
    {
        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams
        {
            var vertexIndex = 4 * i;
            var triangleIndex = 2 * i;
            
            var offsetY = i / Resolution;
            var offsetX = (i % (Resolution));
            
            float xBase = offsetX;
            float yBase = offsetY;
            
            float x = xBase + WidthWithMargin;
            float y = yBase + HeightWithMargin;
            
            var vertex = new Vertex();
            vertex.normal.z = -1f;
            vertex.tangent.xw = float2(1f, -1f);

            vertex.position = new float3(xBase, yBase,0f);
            streams.SetVertex(vertexIndex + 0, vertex);

            vertex.position = new float3(x, yBase,0f);
            vertex.texCoord0 = float2(1f, 0f);
            streams.SetVertex(vertexIndex + 1, vertex);

            vertex.position = new float3(xBase, y,0f);
            vertex.texCoord0 = float2(0f, 1f);
            streams.SetVertex(vertexIndex + 2, vertex);

            vertex.position = new float3(x, y,0f);
            vertex.texCoord0 = 1f;
            streams.SetVertex(vertexIndex + 3, vertex);
            
            streams.SetTriangle(triangleIndex + 0, vertexIndex + int3(0, 2, 1));
            streams.SetTriangle(triangleIndex + 1, vertexIndex + int3(1, 2, 3));
        }

        public float Width => 1.0f;
        public float Height => 1.0f;
        public float WidthWithMargin => 0.9f * Width;
        public float HeightWithMargin => 0.9f * Height;
        public Bounds Bounds { get=> new Bounds(new Vector3(Width/2.0f, Height/2.0f), new Vector3(Width, Height)); }
        public int Resolution { get=>math.max(1,mResolution); set=>mResolution = value; }
        private int mResolution;
    }
    
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<G, S> : IJobFor
        where G : struct, IMeshGenerator
        where S : struct, IMeshStreams
    {
        G generator;
        
        //间接将只写状态应用于IMeshStreams实现所包含的本机数组
        [WriteOnly]
        S streams;

        public void Execute(int i)
        {
            generator.Execute(i, streams);
        }
        
        public static JobHandle ScheduleParallel (
            Mesh mesh, Mesh.MeshData meshData, int resolution, JobHandle dependency
        ) {
            var job = new MeshJob<G, S>();
            mesh.bounds = new Bounds(job.generator.Bounds.center * resolution, job.generator.Bounds.size * resolution);
            job.generator.Resolution = resolution;
            job.streams.Setup(
                meshData
                , mesh.bounds
                , job.generator.VertexCount
                , job.generator.IndexCount
            );
            return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
        }

    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUInt16 {
		
        public ushort a, b, c;

        public static implicit operator TriangleUInt16 (int3 t) => new TriangleUInt16 {
            a = (ushort)t.x,
            b = (ushort)t.y,
            c = (ushort)t.z
        };
    }
    
    
}

