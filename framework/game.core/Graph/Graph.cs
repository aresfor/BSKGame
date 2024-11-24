using Game.Math;

namespace Game.Core;


public interface IGraph<T>
{
    /// <summary>
    /// 根据最大距离distance返回所有节点
    /// </summary>
    void BFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int distance);
    void DFS(List<IGraphNode<T>> resultNodes,IGraphNode<T> node, int depth);

    /// <summary>
    /// 适用于无权图的BFS
    /// </summary>

    bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path);

    public bool BFS(float3 startWorldPos, float3 goalWorldPos, List<float3> path);
    
    void GetAllNodes(List<IGraphNode<T>> resultNodes);
    void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node);

    IGraphNode<T> FindNode(IGraphNodeHandle handle);

    // IGraphNode<T> IGraph.FindNode<T>(IGraphNodeHandle handle)
    // {
    //     if(handle is IGraphNodeHandleBase<IGraph> tHandle)
    //         return (IGraphNode<T>)FindNode(tHandle);
    //     return null;
    // }

    void Clear();

    void ForEach(Action<IGraphNode<T>> action);
    bool WorldToGraph(float3 worldPos, out IGraphNode<T> node);
    bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition);

    bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos);
    float3 GetWorldPosition();

}

public abstract class GraphBase<T> : IGraph<T>
{
    public abstract void BFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int distance);
    public abstract void DFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int depth);
    public abstract bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path);

    public virtual bool BFS(float3 startWorldPos, float3 goalWorldPos, List<float3> path)
    {
            if (path == null)
                return false;
        
            if (false == WorldToGraph(startWorldPos, out IGraphNode<T> startNode))
                return false;
        
            if (false == WorldToGraph(goalWorldPos, out IGraphNode<T> goalNode))
                return false;

            path.Clear();
            List<IGraphNode<T>> resultNodes = ListPool<IGraphNode<T>>.Get();
            bool result = BFS(startNode, goalNode, resultNodes);
            if (result)
            {
                foreach (var resultNode in resultNodes)
                {
                    path.Add(resultNode.WorldPosition);
                }
            }

            resultNodes.Clear();
            ListPool<IGraphNode<T>>.Release(resultNodes);
            return result;
    }
    public abstract void GetAllNodes(List<IGraphNode<T>> resultNodes);
    public abstract void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node);
    public abstract IGraphNode<T> FindNode(IGraphNodeHandle handle);
    public abstract void Clear();
    public abstract void ForEach(Action<IGraphNode<T>> action);
    public abstract bool WorldToGraph(float3 worldPos, out IGraphNode<T> node);
    public abstract bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition);
    public abstract bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos);
    public abstract float3 GetWorldPosition();
}