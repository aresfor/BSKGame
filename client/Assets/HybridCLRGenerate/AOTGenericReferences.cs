using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"BSK.Game.Common.dll",
		"BSK.Game.Core.dll",
		"QFramework.dll",
		"System.Core.dll",
		"System.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.TilemapModule.dll",
		"UnityGameFramework.Runtime.dll",
		"game.core.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Game.Core.AbstractCommand<object>
	// Game.Core.AbstractQuery<int>
	// Game.Core.Architecture.<>c<object>
	// Game.Core.Architecture<object>
	// Game.Core.ArrayGraph<object>
	// Game.Core.BindableProperty.<>c<float>
	// Game.Core.BindableProperty.<>c<int>
	// Game.Core.BindableProperty.<>c<object>
	// Game.Core.BindableProperty.<>c__DisplayClass21_0<float>
	// Game.Core.BindableProperty.<>c__DisplayClass21_0<int>
	// Game.Core.BindableProperty.<>c__DisplayClass21_0<object>
	// Game.Core.BindableProperty<float>
	// Game.Core.BindableProperty<int>
	// Game.Core.BindableProperty<object>
	// Game.Core.CollectionPool.<>c<object,Game.Math.float3>
	// Game.Core.CollectionPool.<>c<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.CollectionPool.<>c<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.CollectionPool.<>c<object,int>
	// Game.Core.CollectionPool.<>c<object,object>
	// Game.Core.CollectionPool<object,Game.Math.float3>
	// Game.Core.CollectionPool<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.CollectionPool<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.CollectionPool<object,int>
	// Game.Core.CollectionPool<object,object>
	// Game.Core.DictionaryPathStorage<object>
	// Game.Core.EasyEvent.<>c<QFramework.Example.InterfaceEventA>
	// Game.Core.EasyEvent.<>c<QFramework.Example.InterfaceEventB>
	// Game.Core.EasyEvent.<>c<QFramework.Example.TypeEventSystemBasicExample.TestEventA>
	// Game.Core.EasyEvent.<>c<QFramework.Example.TypeEventSystemInheritEventExample.EventB>
	// Game.Core.EasyEvent.<>c<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>
	// Game.Core.EasyEvent.<>c<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>
	// Game.Core.EasyEvent.<>c<float,float>
	// Game.Core.EasyEvent.<>c<float>
	// Game.Core.EasyEvent.<>c<int,int>
	// Game.Core.EasyEvent.<>c<int>
	// Game.Core.EasyEvent.<>c<object,object>
	// Game.Core.EasyEvent.<>c<object>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.InterfaceEventA>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.InterfaceEventB>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.TypeEventSystemBasicExample.TestEventA>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.TypeEventSystemInheritEventExample.EventB>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<float,float>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<float>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<int,int>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<int>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<object,object>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<object>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.InterfaceEventA>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.InterfaceEventB>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.TypeEventSystemBasicExample.TestEventA>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.TypeEventSystemInheritEventExample.EventB>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<float,float>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<float>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<int,int>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<int>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<object,object>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<object>
	// Game.Core.EasyEvent<QFramework.Example.InterfaceEventA>
	// Game.Core.EasyEvent<QFramework.Example.InterfaceEventB>
	// Game.Core.EasyEvent<QFramework.Example.TypeEventSystemBasicExample.TestEventA>
	// Game.Core.EasyEvent<QFramework.Example.TypeEventSystemInheritEventExample.EventB>
	// Game.Core.EasyEvent<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>
	// Game.Core.EasyEvent<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>
	// Game.Core.EasyEvent<float,float>
	// Game.Core.EasyEvent<float>
	// Game.Core.EasyEvent<int,int>
	// Game.Core.EasyEvent<int>
	// Game.Core.EasyEvent<object,object>
	// Game.Core.EasyEvent<object>
	// Game.Core.FPoolWrapper<object,Game.Math.float3>
	// Game.Core.FPoolWrapper<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.FPoolWrapper<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.FPoolWrapper<object,object>
	// Game.Core.GraphBase<object>
	// Game.Core.GraphNodeBase<object,object>
	// Game.Core.IBindableProperty<float>
	// Game.Core.ICommand<object>
	// Game.Core.IGraph<object>
	// Game.Core.IGraphNode<object>
	// Game.Core.IOnEvent<QFramework.Example.InterfaceEventA>
	// Game.Core.IOnEvent<QFramework.Example.InterfaceEventB>
	// Game.Core.IReadonlyBindableProperty<float>
	// Game.Core.MonoSingleton<object>
	// Game.Core.PriorityQueue.<>c__DisplayClass6_0<object>
	// Game.Core.PriorityQueue<object>
	// GameFramework.DataTable.IDataTable<object>
	// GameFramework.GameFrameworkLinkedList.Enumerator<object>
	// GameFramework.GameFrameworkLinkedList<object>
	// GameFramework.ObjectPool.IObjectPool<object>
	// GameFramework.Variable<object>
	// GameFramework.Variable<uint>
	// SerializableDictionary<object,object>
	// SerializableDictionaryBase.Dictionary<object,object>
	// SerializableDictionaryBase<object,object,object>
	// System.Action<Game.Math.float3>
	// System.Action<QFramework.Example.InterfaceEventA>
	// System.Action<QFramework.Example.InterfaceEventB>
	// System.Action<QFramework.Example.TypeEventSystemBasicExample.TestEventA>
	// System.Action<QFramework.Example.TypeEventSystemInheritEventExample.EventB>
	// System.Action<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>
	// System.Action<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>
	// System.Action<SnakeGame.CreateFoodEvent>
	// System.Action<SnakeGame.CreateGridEvent>
	// System.Action<SnakeGame.DirInputEvent>
	// System.Action<SnakeGame.EatFoodEvent>
	// System.Action<SnakeGame.GameInitEndEvent>
	// System.Action<SnakeGame.GameOverEvent>
	// System.Action<SnakeGame.SnakeBiggerEvent>
	// System.Action<SnakeGame.SnakeMoveEvent>
	// System.Action<SnakeGame.SnakePosUpdateEvent>
	// System.Action<System.ValueTuple<object,float>>
	// System.Action<UnityEngine.Vector3Int>
	// System.Action<byte>
	// System.Action<float,float>
	// System.Action<float>
	// System.Action<int,int>
	// System.Action<int>
	// System.Action<object,object>
	// System.Action<object>
	// System.Action<uint>
	// System.ArraySegment.Enumerator<Game.Client.Stream0>
	// System.ArraySegment.Enumerator<Game.Client.TriangleUInt16>
	// System.ArraySegment.Enumerator<Unity.Mathematics.float2>
	// System.ArraySegment.Enumerator<Unity.Mathematics.float3>
	// System.ArraySegment.Enumerator<Unity.Mathematics.float4>
	// System.ArraySegment.Enumerator<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.ArraySegment.Enumerator<ushort>
	// System.ArraySegment<Game.Client.Stream0>
	// System.ArraySegment<Game.Client.TriangleUInt16>
	// System.ArraySegment<Unity.Mathematics.float2>
	// System.ArraySegment<Unity.Mathematics.float3>
	// System.ArraySegment<Unity.Mathematics.float4>
	// System.ArraySegment<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.ArraySegment<ushort>
	// System.ByReference<Game.Client.Stream0>
	// System.ByReference<Game.Client.TriangleUInt16>
	// System.ByReference<Unity.Mathematics.float2>
	// System.ByReference<Unity.Mathematics.float3>
	// System.ByReference<Unity.Mathematics.float4>
	// System.ByReference<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.ByReference<ushort>
	// System.Collections.Generic.ArraySortHelper<Game.Math.float3>
	// System.Collections.Generic.ArraySortHelper<System.ValueTuple<object,float>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3Int>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<uint>
	// System.Collections.Generic.Comparer<Game.Math.float3>
	// System.Collections.Generic.Comparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<uint>
	// System.Collections.Generic.Dictionary.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.Enumerator<byte,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<byte,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<byte,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<byte,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<byte,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<uint,object>
	// System.Collections.Generic.Dictionary<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary<byte,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<uint,object>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.HashSet.Enumerator<byte>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<byte>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<byte>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Game.Math.float3>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<byte,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,float>>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3Int>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<uint>
	// System.Collections.Generic.IComparer<Game.Math.float3>
	// System.Collections.Generic.IComparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<uint>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<Game.Math.float3>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<byte,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,float>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<uint>
	// System.Collections.Generic.IEnumerator<Game.Math.float3>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<byte,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,float>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<uint>
	// System.Collections.Generic.IEqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEqualityComparer<byte>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<uint>
	// System.Collections.Generic.IList<Game.Math.float3>
	// System.Collections.Generic.IList<System.ValueTuple<object,float>>
	// System.Collections.Generic.IList<UnityEngine.Vector3Int>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<uint>
	// System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.KeyValuePair<byte,object>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<uint,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<Game.Math.float3>
	// System.Collections.Generic.List.Enumerator<System.ValueTuple<object,float>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3Int>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<uint>
	// System.Collections.Generic.List<Game.Math.float3>
	// System.Collections.Generic.List<System.ValueTuple<object,float>>
	// System.Collections.Generic.List<UnityEngine.Vector3Int>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<uint>
	// System.Collections.Generic.ObjectComparer<Game.Math.float3>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.Queue.Enumerator<Game.Math.float3>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<Game.Math.float3>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<Game.Math.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.ValueTuple<object,float>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3Int>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<uint>
	// System.Comparison<Game.Math.float3>
	// System.Comparison<System.ValueTuple<object,float>>
	// System.Comparison<UnityEngine.Vector3Int>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Comparison<uint>
	// System.EventHandler<object>
	// System.Func<Game.Gameplay.GenericTeamId,Game.Gameplay.GenericTeamId,byte>
	// System.Func<byte>
	// System.Func<float,float,byte>
	// System.Func<int,int,byte>
	// System.Func<int,int>
	// System.Func<object,byte>
	// System.Func<object,object,byte>
	// System.Func<object,object,object>
	// System.Func<object>
	// System.IEquatable<Game.Gameplay.GenericTeamId>
	// System.Lazy<object>
	// System.Linq.Buffer<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.EnumerableSorter<int,int>
	// System.Linq.EnumerableSorter<int>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<int>
	// System.Linq.OrderedEnumerable<int,int>
	// System.Linq.OrderedEnumerable<int>
	// System.Nullable<int>
	// System.Predicate<Game.Math.float3>
	// System.Predicate<System.ValueTuple<object,float>>
	// System.Predicate<UnityEngine.Vector3Int>
	// System.Predicate<byte>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Predicate<uint>
	// System.ReadOnlySpan.Enumerator<Game.Client.Stream0>
	// System.ReadOnlySpan.Enumerator<Game.Client.TriangleUInt16>
	// System.ReadOnlySpan.Enumerator<Unity.Mathematics.float2>
	// System.ReadOnlySpan.Enumerator<Unity.Mathematics.float3>
	// System.ReadOnlySpan.Enumerator<Unity.Mathematics.float4>
	// System.ReadOnlySpan.Enumerator<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.ReadOnlySpan.Enumerator<ushort>
	// System.ReadOnlySpan<Game.Client.Stream0>
	// System.ReadOnlySpan<Game.Client.TriangleUInt16>
	// System.ReadOnlySpan<Unity.Mathematics.float2>
	// System.ReadOnlySpan<Unity.Mathematics.float3>
	// System.ReadOnlySpan<Unity.Mathematics.float4>
	// System.ReadOnlySpan<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.ReadOnlySpan<ushort>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<byte>
	// System.Runtime.CompilerServices.TaskAwaiter<byte>
	// System.Span.Enumerator<Game.Client.Stream0>
	// System.Span.Enumerator<Game.Client.TriangleUInt16>
	// System.Span.Enumerator<Unity.Mathematics.float2>
	// System.Span.Enumerator<Unity.Mathematics.float3>
	// System.Span.Enumerator<Unity.Mathematics.float4>
	// System.Span.Enumerator<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.Span.Enumerator<ushort>
	// System.Span<Game.Client.Stream0>
	// System.Span<Game.Client.TriangleUInt16>
	// System.Span<Unity.Mathematics.float2>
	// System.Span<Unity.Mathematics.float3>
	// System.Span<Unity.Mathematics.float4>
	// System.Span<UnityEngine.Rendering.VertexAttributeDescriptor>
	// System.Span<ushort>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<byte>
	// System.Threading.Tasks.Task<byte>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<byte>
	// System.Threading.Tasks.TaskFactory<byte>
	// System.ValueTuple<object,float>
	// Unity.Collections.LowLevel.Unsafe.BurstLike.SharedStatic<System.IntPtr>
	// Unity.Collections.NativeArray.Enumerator<Game.Client.Stream0>
	// Unity.Collections.NativeArray.Enumerator<Game.Client.TriangleUInt16>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.float3>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.Enumerator<UnityEngine.Rendering.VertexAttributeDescriptor>
	// Unity.Collections.NativeArray.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Game.Client.Stream0>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Game.Client.TriangleUInt16>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.float3>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<UnityEngine.Rendering.VertexAttributeDescriptor>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly<Game.Client.Stream0>
	// Unity.Collections.NativeArray.ReadOnly<Game.Client.TriangleUInt16>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.float3>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.ReadOnly<UnityEngine.Rendering.VertexAttributeDescriptor>
	// Unity.Collections.NativeArray.ReadOnly<ushort>
	// Unity.Collections.NativeArray<Game.Client.Stream0>
	// Unity.Collections.NativeArray<Game.Client.TriangleUInt16>
	// Unity.Collections.NativeArray<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray<Unity.Mathematics.float3>
	// Unity.Collections.NativeArray<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray<UnityEngine.Rendering.VertexAttributeDescriptor>
	// Unity.Collections.NativeArray<ushort>
	// Unity.Jobs.IJobForExtensions.ForJobStruct.ExecuteJobFunction<Game.Client.MeshJob<object,object>>
	// Unity.Jobs.IJobForExtensions.ForJobStruct<Game.Client.MeshJob<object,object>>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// Game.Core.IUnRegister Game.Client.UnRegisterExtension.UnRegisterWhenDisabled<object>(Game.Core.IUnRegister,object)
		// Game.Core.IUnRegister Game.Client.UnRegisterExtension.UnRegisterWhenGameObjectDestroyed<object>(Game.Core.IUnRegister,object)
		// object Game.Core.Architecture<object>.ExecuteCommand<object>(Game.Core.ICommand<object>)
		// System.Void Game.Core.Architecture<object>.RegisterModel<object>(object)
		// System.Void Game.Core.Architecture<object>.RegisterSystem<object>(object)
		// System.Void Game.Core.Architecture<object>.RegisterUtility<object>(object)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.CreateFoodEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.CreateFoodEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.CreateGridEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.CreateGridEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.DirInputEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.DirInputEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.EatFoodEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.EatFoodEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.GameInitEndEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.GameInitEndEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.GameOverEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.GameOverEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.SnakeBiggerEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.SnakeBiggerEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.SnakeMoveEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.SnakeMoveEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<SnakeGame.SnakePosUpdateEvent>(Game.Core.ICanRegisterEvent,System.Action<SnakeGame.SnakePosUpdateEvent>)
		// Game.Core.IUnRegister Game.Core.CanRegisterEventExtension.RegisterEvent<object>(Game.Core.ICanRegisterEvent,System.Action<object>)
		// System.Void Game.Core.CanRegisterEventExtension.UnRegisterEvent<object>(Game.Core.ICanRegisterEvent,System.Action<object>)
		// System.Void Game.Core.CanSendCommandExtension.SendCommand<object>(Game.Core.ICanSendCommand)
		// System.Void Game.Core.CanSendCommandExtension.SendCommand<object>(Game.Core.ICanSendCommand,object)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.CreateFoodEvent>(Game.Core.ICanSendEvent,SnakeGame.CreateFoodEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.CreateGridEvent>(Game.Core.ICanSendEvent,SnakeGame.CreateGridEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.DirInputEvent>(Game.Core.ICanSendEvent,SnakeGame.DirInputEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.EatFoodEvent>(Game.Core.ICanSendEvent,SnakeGame.EatFoodEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.GameInitEndEvent>(Game.Core.ICanSendEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.GameOverEvent>(Game.Core.ICanSendEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.SnakeBiggerEvent>(Game.Core.ICanSendEvent,SnakeGame.SnakeBiggerEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.SnakeMoveEvent>(Game.Core.ICanSendEvent,SnakeGame.SnakeMoveEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<SnakeGame.SnakePosUpdateEvent>(Game.Core.ICanSendEvent,SnakeGame.SnakePosUpdateEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<object>(Game.Core.ICanSendEvent)
		// System.Void Game.Core.CanSendEventExtension.SendEvent<object>(Game.Core.ICanSendEvent,object)
		// int Game.Core.CanSendQueryExtension.SendQuery<int>(Game.Core.ICanSendQuery,Game.Core.IQuery<int>)
		// object Game.Core.EasyEvents.GetEvent<object>()
		// object Game.Core.EasyEvents.GetOrAddEvent<object>()
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.CreateFoodEvent>(System.Action<SnakeGame.CreateFoodEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.CreateGridEvent>(System.Action<SnakeGame.CreateGridEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.DirInputEvent>(System.Action<SnakeGame.DirInputEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.EatFoodEvent>(System.Action<SnakeGame.EatFoodEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.GameInitEndEvent>(System.Action<SnakeGame.GameInitEndEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.GameOverEvent>(System.Action<SnakeGame.GameOverEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.SnakeBiggerEvent>(System.Action<SnakeGame.SnakeBiggerEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.SnakeMoveEvent>(System.Action<SnakeGame.SnakeMoveEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<SnakeGame.SnakePosUpdateEvent>(System.Action<SnakeGame.SnakePosUpdateEvent>)
		// Game.Core.IUnRegister Game.Core.IArchitecture.RegisterEvent<object>(System.Action<object>)
		// System.Void Game.Core.IArchitecture.SendCommand<object>(object)
		// object Game.Core.IArchitecture.SendCommand<object>(Game.Core.ICommand<object>)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.CreateFoodEvent>(SnakeGame.CreateFoodEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.CreateGridEvent>(SnakeGame.CreateGridEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.DirInputEvent>(SnakeGame.DirInputEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.EatFoodEvent>(SnakeGame.EatFoodEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.GameInitEndEvent>()
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.GameOverEvent>()
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.SnakeBiggerEvent>(SnakeGame.SnakeBiggerEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.SnakeMoveEvent>(SnakeGame.SnakeMoveEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<SnakeGame.SnakePosUpdateEvent>(SnakeGame.SnakePosUpdateEvent)
		// System.Void Game.Core.IArchitecture.SendEvent<object>()
		// System.Void Game.Core.IArchitecture.SendEvent<object>(object)
		// int Game.Core.IArchitecture.SendQuery<int>(Game.Core.IQuery<int>)
		// System.Void Game.Core.IArchitecture.UnRegisterEvent<object>(System.Action<object>)
		// System.Void Game.Core.IOCContainer.Register<object>(object)
		// Game.Core.IUnRegister Game.Core.OnGlobalEventExtension.RegisterEvent<QFramework.Example.InterfaceEventA>(Game.Core.IOnEvent<QFramework.Example.InterfaceEventA>)
		// Game.Core.IUnRegister Game.Core.OnGlobalEventExtension.RegisterEvent<QFramework.Example.InterfaceEventB>(Game.Core.IOnEvent<QFramework.Example.InterfaceEventB>)
		// System.Void Game.Core.OnGlobalEventExtension.UnRegisterEvent<QFramework.Example.InterfaceEventB>(Game.Core.IOnEvent<QFramework.Example.InterfaceEventB>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<QFramework.Example.InterfaceEventA>(System.Action<QFramework.Example.InterfaceEventA>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<QFramework.Example.InterfaceEventB>(System.Action<QFramework.Example.InterfaceEventB>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<QFramework.Example.TypeEventSystemBasicExample.TestEventA>(System.Action<QFramework.Example.TypeEventSystemBasicExample.TestEventA>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>(System.Action<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>(System.Action<QFramework.Example.TypeEventSystemUnRegisterExample.EventB>)
		// Game.Core.IUnRegister Game.Core.TypeEventSystem.Register<object>(System.Action<object>)
		// System.Void Game.Core.TypeEventSystem.Send<QFramework.Example.InterfaceEventA>()
		// System.Void Game.Core.TypeEventSystem.Send<QFramework.Example.InterfaceEventB>()
		// System.Void Game.Core.TypeEventSystem.Send<QFramework.Example.TypeEventSystemBasicExample.TestEventA>()
		// System.Void Game.Core.TypeEventSystem.Send<QFramework.Example.TypeEventSystemBasicExample.TestEventA>(QFramework.Example.TypeEventSystemBasicExample.TestEventA)
		// System.Void Game.Core.TypeEventSystem.Send<QFramework.Example.TypeEventSystemInheritEventExample.EventB>()
		// System.Void Game.Core.TypeEventSystem.Send<object>(object)
		// System.Void Game.Core.TypeEventSystem.UnRegister<QFramework.Example.InterfaceEventB>(System.Action<QFramework.Example.InterfaceEventB>)
		// System.Void Game.Core.TypeEventSystem.UnRegister<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>(System.Action<QFramework.Example.TypeEventSystemUnRegisterExample.EventA>)
		// GameFramework.DataTable.IDataTable<object> GameFramework.DataTable.IDataTableManager.GetDataTable<object>()
		// System.Void GameFramework.GameFrameworkLog.Error<object,object>(string,object,object)
		// System.Void GameFramework.GameFrameworkLog.Info<object,object,object,object>(string,object,object,object,object)
		// System.Void GameFramework.GameFrameworkLog.Info<object,object>(string,object,object)
		// System.Void GameFramework.GameFrameworkLog.Info<object>(string,object)
		// System.Void GameFramework.GameFrameworkLog.Warning<object>(string,object)
		// GameFramework.ObjectPool.IObjectPool<object> GameFramework.ObjectPool.IObjectPoolManager.CreateSingleSpawnObjectPool<object>(string,float,int,float,int)
		// GameFramework.ObjectPool.IObjectPool<object> GameFramework.ObjectPool.IObjectPoolManager.CreateSingleSpawnObjectPool<object>(string,int)
		// object GameFramework.Utility.Json.ToObject<object>(string)
		// object GameFramework.Utility.Json.IJsonHelper.ToObject<object>(string)
		// string GameFramework.Utility.Text.Format<int>(string,int)
		// string GameFramework.Utility.Text.Format<object,object,object,object>(string,object,object,object,object)
		// string GameFramework.Utility.Text.Format<object,object>(string,object,object)
		// string GameFramework.Utility.Text.Format<object>(string,object)
		// string GameFramework.Utility.Text.ITextHelper.Format<int>(string,int)
		// string GameFramework.Utility.Text.ITextHelper.Format<object,object,object,object>(string,object,object,object,object)
		// string GameFramework.Utility.Text.ITextHelper.Format<object,object>(string,object,object)
		// string GameFramework.Utility.Text.ITextHelper.Format<object>(string,object)
		// QFramework.Example.InterfaceEventA System.Activator.CreateInstance<QFramework.Example.InterfaceEventA>()
		// QFramework.Example.InterfaceEventB System.Activator.CreateInstance<QFramework.Example.InterfaceEventB>()
		// QFramework.Example.TypeEventSystemBasicExample.TestEventA System.Activator.CreateInstance<QFramework.Example.TypeEventSystemBasicExample.TestEventA>()
		// QFramework.Example.TypeEventSystemInheritEventExample.EventB System.Activator.CreateInstance<QFramework.Example.TypeEventSystemInheritEventExample.EventB>()
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// int System.Linq.Enumerable.Count<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Linq.IOrderedEnumerable<int> System.Linq.Enumerable.OrderBy<int,int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,int>)
		// int[] System.Linq.Enumerable.ToArray<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,QFramework.Example.CommandWithResultExample.TaskACommand.<OnExecute>d__0>(System.Runtime.CompilerServices.TaskAwaiter&,QFramework.Example.CommandWithResultExample.TaskACommand.<OnExecute>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<QFramework.Example.CommandWithResultExample.TaskACommand.<OnExecute>d__0>(QFramework.Example.CommandWithResultExample.TaskACommand.<OnExecute>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,QFramework.Example.UnRegisterWhenCurrentSceneUnloadedExample.<Start>d__2>(System.Runtime.CompilerServices.TaskAwaiter&,QFramework.Example.UnRegisterWhenCurrentSceneUnloadedExample.<Start>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,QFramework.PointGame.AchievementSystem.<<OnInit>b__2_2>d>(System.Runtime.CompilerServices.TaskAwaiter&,QFramework.PointGame.AchievementSystem.<<OnInit>b__2_2>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,QFramework.Example.CommandWithResultExample.<SendTaskACommand>d__5>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,QFramework.Example.CommandWithResultExample.<SendTaskACommand>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<QFramework.Example.CommandWithResultExample.<SendTaskACommand>d__5>(QFramework.Example.CommandWithResultExample.<SendTaskACommand>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<QFramework.Example.UnRegisterWhenCurrentSceneUnloadedExample.<Start>d__2>(QFramework.Example.UnRegisterWhenCurrentSceneUnloadedExample.<Start>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<QFramework.PointGame.AchievementSystem.<<OnInit>b__2_2>d>(QFramework.PointGame.AchievementSystem.<<OnInit>b__2_2>d&)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// Unity.Collections.NativeArray<Game.Client.Stream0> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Game.Client.Stream0>(System.Void*,int,Unity.Collections.Allocator)
		// Unity.Collections.NativeArray<Game.Client.TriangleUInt16> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Game.Client.TriangleUInt16>(System.Void*,int,Unity.Collections.Allocator)
		// Unity.Collections.NativeArray<Unity.Mathematics.float2> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Unity.Mathematics.float2>(System.Void*,int,Unity.Collections.Allocator)
		// Unity.Collections.NativeArray<Unity.Mathematics.float3> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Unity.Mathematics.float3>(System.Void*,int,Unity.Collections.Allocator)
		// Unity.Collections.NativeArray<Unity.Mathematics.float4> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Unity.Mathematics.float4>(System.Void*,int,Unity.Collections.Allocator)
		// Unity.Collections.NativeArray<ushort> Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ushort>(System.Void*,int,Unity.Collections.Allocator)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<Game.Client.MeshJob<object,object>>(Game.Client.MeshJob<object,object>&)
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Game.Client.Stream0>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Game.Client.TriangleUInt16>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Unity.Mathematics.float2>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Unity.Mathematics.float3>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Unity.Mathematics.float4>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<ushort>()
		// Unity.Collections.NativeArray<Game.Client.TriangleUInt16> Unity.Collections.NativeArray<ushort>.InternalReinterpret<Game.Client.TriangleUInt16>(int)
		// Unity.Collections.NativeArray<Game.Client.TriangleUInt16> Unity.Collections.NativeArray<ushort>.Reinterpret<Game.Client.TriangleUInt16>(int)
		// System.IntPtr Unity.Jobs.IJobForExtensions.GetReflectionData<Game.Client.MeshJob<object,object>>()
		// Unity.Jobs.JobHandle Unity.Jobs.IJobForExtensions.ScheduleParallel<Game.Client.MeshJob<object,object>>(Game.Client.MeshJob<object,object>,int,int,Unity.Jobs.JobHandle)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// System.Void UnityEngine.Component.GetComponentsInChildren<object>(bool,System.Collections.Generic.List<object>)
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>(bool)
		// System.Void UnityEngine.GameObject.GetComponentsInChildren<object>(bool,System.Collections.Generic.List<object>)
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// Unity.Collections.NativeArray<ushort> UnityEngine.Mesh.MeshData.GetIndexData<ushort>()
		// Unity.Collections.NativeArray<Game.Client.Stream0> UnityEngine.Mesh.MeshData.GetVertexData<Game.Client.Stream0>(int)
		// Unity.Collections.NativeArray<Unity.Mathematics.float2> UnityEngine.Mesh.MeshData.GetVertexData<Unity.Mathematics.float2>(int)
		// Unity.Collections.NativeArray<Unity.Mathematics.float3> UnityEngine.Mesh.MeshData.GetVertexData<Unity.Mathematics.float3>(int)
		// Unity.Collections.NativeArray<Unity.Mathematics.float4> UnityEngine.Mesh.MeshData.GetVertexData<Unity.Mathematics.float4>(int)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Resources.Load<object>(string)
		// object UnityEngine.Tilemaps.Tilemap.GetTile<object>(UnityEngine.Vector3Int)
		// object UnityExtension.GetOrAddComponent<object>(UnityEngine.GameObject)
		// GameFramework.DataTable.IDataTable<object> UnityGameFramework.Runtime.DataTableComponent.GetDataTable<object>()
		// object UnityGameFramework.Runtime.GameEntry.GetComponent<object>()
		// System.Void UnityGameFramework.Runtime.Log.Error<object,object>(string,object,object)
		// System.Void UnityGameFramework.Runtime.Log.Info<object,object,object,object>(string,object,object,object,object)
		// System.Void UnityGameFramework.Runtime.Log.Info<object,object>(string,object,object)
		// System.Void UnityGameFramework.Runtime.Log.Info<object>(string,object)
		// System.Void UnityGameFramework.Runtime.Log.Warning<object>(string,object)
		// GameFramework.ObjectPool.IObjectPool<object> UnityGameFramework.Runtime.ObjectPoolComponent.CreateSingleSpawnObjectPool<object>(string,float,int,float,int)
		// GameFramework.ObjectPool.IObjectPool<object> UnityGameFramework.Runtime.ObjectPoolComponent.CreateSingleSpawnObjectPool<object>(string,int)
	}
}