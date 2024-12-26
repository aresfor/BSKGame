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
	// Game.Core.Architecture.<>c<object>
	// Game.Core.Architecture<object>
	// Game.Core.ArrayGraph<object>
	// Game.Core.BindableProperty.<>c<object>
	// Game.Core.BindableProperty.<>c__DisplayClass21_0<object>
	// Game.Core.BindableProperty<object>
	// Game.Core.CollectionPool.<>c<object,Game.Math.float3>
	// Game.Core.CollectionPool.<>c<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.CollectionPool.<>c<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.CollectionPool.<>c<object,object>
	// Game.Core.CollectionPool<object,Game.Math.float3>
	// Game.Core.CollectionPool<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.CollectionPool<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.CollectionPool<object,object>
	// Game.Core.DictionaryPathStorage<object>
	// Game.Core.EasyEvent.<>c<object,object>
	// Game.Core.EasyEvent.<>c<object>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<object,object>
	// Game.Core.EasyEvent.<>c__DisplayClass1_0<object>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<object,object>
	// Game.Core.EasyEvent.<>c__DisplayClass4_0<object>
	// Game.Core.EasyEvent<object,object>
	// Game.Core.EasyEvent<object>
	// Game.Core.FPoolWrapper<object,Game.Math.float3>
	// Game.Core.FPoolWrapper<object,System.Collections.Generic.KeyValuePair<object,float>>
	// Game.Core.FPoolWrapper<object,System.Collections.Generic.KeyValuePair<object,object>>
	// Game.Core.FPoolWrapper<object,object>
	// Game.Core.GraphBase<object>
	// Game.Core.GraphNodeBase<object,object>
	// Game.Core.IGraph<object>
	// Game.Core.IGraphNode<object>
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
	// System.Action<System.ValueTuple<object,float>>
	// System.Action<UnityEngine.Vector3Int>
	// System.Action<byte>
	// System.Action<float,float>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<Game.Math.float3>
	// System.Collections.Generic.ArraySortHelper<System.ValueTuple<object,float>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3Int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<Game.Math.float3>
	// System.Collections.Generic.Comparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.KeyCollection<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<uint,object>
	// System.Collections.Generic.Dictionary.ValueCollection<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<uint,object>
	// System.Collections.Generic.Dictionary<UnityEngine.Vector3Int,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<uint,object>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Game.Math.float3>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,float>>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3Int>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<Game.Math.float3>
	// System.Collections.Generic.IComparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<Game.Math.float3>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,float>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<Game.Math.float3>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<uint,object>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,float>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<uint>
	// System.Collections.Generic.IList<Game.Math.float3>
	// System.Collections.Generic.IList<System.ValueTuple<object,float>>
	// System.Collections.Generic.IList<UnityEngine.Vector3Int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<UnityEngine.Vector3Int,object>
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
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<Game.Math.float3>
	// System.Collections.Generic.List<System.ValueTuple<object,float>>
	// System.Collections.Generic.List<UnityEngine.Vector3Int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<Game.Math.float3>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<object,float>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3Int>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<Game.Math.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.ValueTuple<object,float>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3Int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<Game.Math.float3>
	// System.Comparison<System.ValueTuple<object,float>>
	// System.Comparison<UnityEngine.Vector3Int>
	// System.Comparison<object>
	// System.EventHandler<object>
	// System.Func<object,byte>
	// System.Func<object,object,byte>
	// System.Func<object>
	// System.Nullable<int>
	// System.Predicate<Game.Math.float3>
	// System.Predicate<System.ValueTuple<object,float>>
	// System.Predicate<UnityEngine.Vector3Int>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.ValueTuple<object,float>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// Game.Core.IUnRegister Game.Client.UnRegisterExtension.UnRegisterWhenDisabled<object>(Game.Core.IUnRegister,object)
		// System.Void Game.Core.Architecture<object>.RegisterModel<object>(object)
		// System.Void Game.Core.Architecture<object>.RegisterSystem<object>(object)
		// System.Void Game.Core.IOCContainer.Register<object>(object)
		// GameFramework.DataTable.IDataTable<object> GameFramework.DataTable.IDataTableManager.GetDataTable<object>()
		// System.Void GameFramework.Fsm.Fsm<object>.ChangeState<object>()
		// System.Void GameFramework.Fsm.FsmState<object>.ChangeState<object>(GameFramework.Fsm.IFsm<object>)
		// object GameFramework.Fsm.IFsm<object>.GetData<object>(string)
		// System.Void GameFramework.Fsm.IFsm<object>.SetData<object>(string,object)
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
		// object System.Activator.CreateInstance<object>()
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
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
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