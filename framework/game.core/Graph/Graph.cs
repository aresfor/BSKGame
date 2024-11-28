using Game.Math;

namespace Game.Core;

public interface IGraph
{
    public bool AStar(float3 startWorldPos, float3 goalWorldPos, List<float3> path);
    void BFS(List<IGraphNode> resultNodes, IGraphNode node, int distance);
    bool WorldToGraph(float3 worldPos, out IGraphNode node);

}

public interface IGraph<T>:IGraph
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
    public bool AStar(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path);
    //public bool AStar(float3 startWorldPos, float3 goalWorldPos, List<float3> path);
    
    bool GetAllNodes(List<IGraphNode<T>> resultNodes);
    void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node);

    IGraphNode<T> FindNode(IGraphNodeHandle handle);
    
    void Clear();

    void ForEach(Action<IGraphNode<T>> action);
    bool WorldToGraph(float3 worldPos, out IGraphNode<T> node);
    bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition);

    bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos);
    float3 GetWorldPosition();

}

public abstract class GraphBase<T> : IGraph<T>
{
    private PriorityQueue<IGraphNode<T>> m_OpenSet = new PriorityQueue<IGraphNode<T>>();
    private Queue<IGraphNode<T>> m_OpenQueue = new Queue<IGraphNode<T>>();
    public abstract void DFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int depth);

    public T GetNodeValue(IGraphNodeHandle handle)
    {
        var node = FindNode(handle);
        if (node != null)
            return node.Value;
        return default;
    }
    private void ReconstructPath(IGraphNode<T>[, ] cameFromArr,  IGraphNode<T> goal, List<IGraphNode<T>> path)
    {
        var current = goal;
        while (current != null)
        {
            var currentHandle = (FArrayGraphNodeHandle)current.Handle;
            path.Insert(0, current);
            current = cameFromArr[currentHandle.Row, currentHandle.Column];
        }
    }
    public void BFS(List<IGraphNode> resultNodes, IGraphNode node, int distance)
    {
        using var resultNodesWithType = new FPoolWrapper<List<IGraphNode<T>>, IGraphNode<T>>();
        BFS(resultNodesWithType.Value, node as IGraphNode<T>, distance);
        
        resultNodes.AddRange(resultNodesWithType.Value);
    }

    public bool WorldToGraph(float3 worldPos, out IGraphNode node)
    {
        bool result =  WorldToGraph(worldPos, out IGraphNode<T> nodeWithType);
        node = nodeWithType;
        return result;
    }

    public void BFS(List<IGraphNode<T>> resultNodes,IGraphNode<T> node, int distance)
    {
        if (resultNodes == null || node == null)
            return;
        
        resultNodes.Clear();
        var queue = m_OpenQueue;
        queue.Clear();
        
        var visitedSet = HashSetPool<IGraphNode<T>>.Get();
        var pendingSet = HashSetPool<IGraphNode<T>>.Get();
        
        queue.Enqueue(node);
        int currentDistance = 0;
        
        var neighbors = ListPool<IGraphNode<T>>.Get();
        while (currentDistance <= distance)
        {
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                visitedSet.Add(currentNode);

                neighbors.Clear();
                if (currentDistance < distance)
                {
                    currentNode.GetNeighbors(neighbors);
                    foreach (var neighborNode in neighbors)
                    {
                        if (neighborNode.IsAvailable)
                        {
                            //还没有访问过
                            if (false == visitedSet.Contains(neighborNode))
                            {
                                pendingSet.Add(neighborNode);
                                // if (neighborNode is IGraphPathNode pathNode)
                                // {
                                //     pathNode.Pre = currentNode;
                                // }
                            }
                        }
                    }
                }
                
            }

            foreach (var pendingNode in pendingSet)
            {
                queue.Enqueue(pendingNode);
            }
            
            pendingSet.Clear();
            
            ++currentDistance;
        }
        
        resultNodes.Capacity = visitedSet.Count;
        foreach (var element in visitedSet)
        {
            //排除起始查询node
            if(element != node)
                resultNodes.Add(element);
        }
        
        neighbors.Clear();
        ListPool<IGraphNode<T>>.Release(neighbors);
        pendingSet.Clear();
        HashSetPool<IGraphNode<T>>.Release(pendingSet);
        visitedSet.Clear();
        HashSetPool<IGraphNode<T>>.Release(visitedSet);
        
    }
    
    public bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path)
    {
        if (path == null || start == null || goal == null)
            return false;

        var visitedSet = HashSetPool<IGraphNode<T>>.Get();
        var queue = m_OpenQueue;
        queue.Clear();

        var pathStorage = new DictionaryPathStorage<T>();
        visitedSet.Add(start);
        queue.Enqueue(start);

        bool result = false;
        var neighbors = ListPool<IGraphNode<T>>.Get();

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (currentNode == goal)
            {
                result = true;
                path.Clear();
                var iterator = goal;
                while (iterator != null)
                {
                    path.Insert(0, iterator);
                    iterator = pathStorage.GetCameFrom(iterator);
                }
                break;
            }

            currentNode.GetNeighbors(neighbors);

            foreach (var neighbor in neighbors)
            {
                if (!neighbor.IsAvailable || visitedSet.Contains(neighbor))
                    continue;

                visitedSet.Add(neighbor);
                pathStorage.SetCameFrom(currentNode, neighbor);
                queue.Enqueue(neighbor);
            }
        }

        neighbors.Clear();
        ListPool<IGraphNode<T>>.Release(neighbors);
        visitedSet.Clear();
        HashSetPool<IGraphNode<T>>.Release(visitedSet);

        return result;
    }
    
    // public bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path)
    // {
    //     if (path == null || start == null || goal == null)
    //         return false;
    //     
    //     var visitedSet = HashSetPool<IGraphNode<T>>.Get();
    //     
    //     m_OpenQueue.Clear();
    //     var queue = m_OpenQueue;
    //
    //     for (int i = 0; i < Array.GetLength(0); ++i)
    //     {
    //         for (int j = 0; j < Array.GetLength(1); ++j)
    //         {
    //             m_CameFromArr[i, j] = null;
    //         }
    //     }
    //
    //     bool result = false;
    //     queue.Enqueue(start);
    //     visitedSet.Add(start);
    //
    //     var neighbors = ListPool<IGraphNode<T>>.Get();
    //     while (queue.Count > 0)
    //     {
    //         var currentNode = queue.Dequeue();
    //
    //         if (currentNode == goal)
    //         {
    //             result = true;
    //             ReconstructPath(m_CameFromArr, goal, path);
    //             break;
    //         }
    //
    //         GetNeighbors(neighbors, currentNode);
    //         for (int i = 0; i < neighbors.Count; ++i)
    //         {
    //             var neighborNode = FindNode(neighbors[i].Handle);
    //             var neighborHandle = (FArrayGraphNodeHandle)neighbors[i].Handle;
    //             if (!visitedSet.Contains(neighborNode))
    //             {
    //                 queue.Enqueue(neighborNode);
    //                 visitedSet.Add(neighborNode);
    //                 m_CameFromArr[neighborHandle.Row, neighborHandle.Column] = currentNode;
    //             }
    //         }
    //     }
    //     
    //     neighbors.Clear();
    //     ListPool<IGraphNode<T>>.Release(neighbors);
    //     visitedSet.Clear();
    //     HashSetPool<IGraphNode<T>>.Release(visitedSet);
    //     
    //     return result;
    // }
    
    
    //public abstract void BFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int distance);
    //public abstract bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path);

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

    protected abstract float Heuristic(IGraphNode<T> a, IGraphNode<T> b);
    
    
    public virtual float Distance(IGraphNode<T> a, IGraphNode<T> b)
    {
        // @注意： 后续可以在这里加入节点的成本之类的
        // 网格相邻节点默认距离为1
        return 1f;
    }
    
    public  bool AStar(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path)
    {
        if (path == null || start == null || goal == null)
            return false;

        var openSet = m_OpenSet;
        openSet.Clear();
            
        using var gScore = new FPoolWrapper<Dictionary<IGraphNode<T>, float>, KeyValuePair<IGraphNode<T>, float>>(); // 从起点到每个节点的实际成本
        using var fScore = new FPoolWrapper<Dictionary<IGraphNode<T>, float>, KeyValuePair<IGraphNode<T>, float>>(); // 总评估成本（g + h）
        //using var cameFrom = new FPoolWrapper<Dictionary<IGraphNode<T>, IGraphNode<T>>, KeyValuePair<IGraphNode<T>, IGraphNode<T>>>(); // 路径记录
        var pathStorage = new DictionaryPathStorage<T>();
        
        // 初始化起点
        openSet.Enqueue(start, 0);
        gScore.Value[start] = 0;
        fScore.Value[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            // 如果找到目标节点，则重建路径
            if (current == goal)
            {
                path.Clear();
                var iterator = goal;
                while (iterator != null)
                {
                    path.Insert(0, iterator);
                    iterator = pathStorage.GetCameFrom(iterator);
                    //cameFrom.Value.TryGetValue(iterator, out iterator);
                }

                return true;
            }

            var neighbors = ListPool<IGraphNode<T>>.Get();
            GetNeighbors(neighbors, current);

            foreach (var neighbor in neighbors)
            {
                if (!neighbor.IsAvailable) continue;

                float tentativeGScore = gScore.Value[current] + Distance(current, neighbor);

                // 如果发现更短的路径
                if (!gScore.Value.ContainsKey(neighbor) || tentativeGScore < gScore.Value[neighbor])
                {
                    pathStorage.SetCameFrom(current, neighbor);
                    //cameFrom.Value[neighbor] = current;
                    gScore.Value[neighbor] = tentativeGScore;
                    fScore.Value[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                    // 如果邻居节点不在开放列表中，加入
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore.Value[neighbor]);
                    }
                }
            }

            neighbors.Clear();
            ListPool<IGraphNode<T>>.Release(neighbors);
        }
        
        // 找不到路径
        return false; 
    }
    public bool AStar(float3 startWorldPos, float3 goalWorldPos, List<float3> path)
    {
        if (path == null)
            return false;
        
        if (false == WorldToGraph(startWorldPos, out IGraphNode<T> startNode))
            return false;
        
        if (false == WorldToGraph(goalWorldPos, out IGraphNode<T> goalNode))
            return false;
        
        path.Clear();
        List<IGraphNode<T>> resultNodes = ListPool<IGraphNode<T>>.Get();
        bool result = AStar(startNode, goalNode, resultNodes);
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

    

    public abstract float3 GetWorldPosition();

    public abstract bool GetAllNodes(List<IGraphNode<T>> resultNodes);
    public abstract void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node);
    public abstract IGraphNode<T> FindNode(IGraphNodeHandle handle);
    public abstract void Clear();
    public abstract void ForEach(Action<IGraphNode<T>> action);
    public abstract bool WorldToGraph(float3 worldPos, out IGraphNode<T> node);
    public abstract bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos);
    public abstract bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition);
    public abstract bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle);
}