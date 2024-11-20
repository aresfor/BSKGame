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
}
