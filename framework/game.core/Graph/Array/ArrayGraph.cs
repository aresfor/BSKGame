using Game.Math;
using GameFramework;

namespace Game.Core;



public abstract class ArrayGraph<T>:  GraphBase<T>
{
    protected IGraphNode<T>[,] Array {  get; }

    private int m_Row;
    private int m_Column;
    public float NodeWidth = 1;
    public float NodeHeight = 1;
    public float NodeMarginWidth => NodeWidth * 0.1f;
    public float NodeMarginHeight => NodeHeight * 0.1f;
    
    private Queue<IGraphNode<T>> m_OpenQueue = new Queue<IGraphNode<T>>();
    private IGraphNode<T>[, ] m_CameFromArr { get; }

    public float3 WorldPosition { get; set; }
    public int Row
    {
        get { return Array.GetLength(0); }
        set => m_Row = value;
    }

    public int Column
    {
        get { return Array.GetLength(1); }
        set => m_Column = value;
    }

    public ArrayGraph(int inRow, int inColumn, float3 graphWorldPosition)
    {
        m_Row = inRow;
        m_Column = inColumn;
        WorldPosition = graphWorldPosition;
        Array = new IGraphNode<T>[m_Row, m_Column];
        m_CameFromArr = new IGraphNode<T>[m_Row, m_Column];
    }
    
    
    
    public override void BFS(List<IGraphNode<T>> resultNodes,IGraphNode<T> node, int distance)
    {
        if (resultNodes == null || node == null)
            return;
        if (Array == null)
        {
            Log.Error("ArrayGraph array is null, check initialize or recycle");
            return;
        }
        
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

    public override void DFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int depth)
    {
        //@TODO:
        throw new NotImplementedException();
    }

    public override bool BFS(IGraphNode<T> start, IGraphNode<T> goal, List<IGraphNode<T>> path)
    {
        if (path == null || start == null || goal == null)
            return false;
        
        var visitedSet = HashSetPool<IGraphNode<T>>.Get();
        
        m_OpenQueue.Clear();
        var queue = m_OpenQueue;

        for (int i = 0; i < Array.GetLength(0); ++i)
        {
            for (int j = 0; j < Array.GetLength(1); ++j)
            {
                m_CameFromArr[i, j] = null;
            }
        }

        bool result = false;
        queue.Enqueue(start);
        visitedSet.Add(start);

        var neighbors = ListPool<IGraphNode<T>>.Get();
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if (currentNode == goal)
            {
                result = true;
                ReconstructPath(m_CameFromArr, goal, path);
                break;
            }

            GetNeighbors(neighbors, currentNode);
            for (int i = 0; i < neighbors.Count; ++i)
            {
                var neighborNode = FindNode(neighbors[i].Handle);
                var neighborHandle = (FArrayGraphNodeHandle)neighbors[i].Handle;
                if (!visitedSet.Contains(neighborNode))
                {
                    queue.Enqueue(neighborNode);
                    visitedSet.Add(neighborNode);
                    m_CameFromArr[neighborHandle.Row, neighborHandle.Column] = currentNode;
                }
            }
        }
        
        neighbors.Clear();
        ListPool<IGraphNode<T>>.Release(neighbors);
        visitedSet.Clear();
        HashSetPool<IGraphNode<T>>.Release(visitedSet);
        
        return result;
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
    
    
    public override void GetAllNodes(List<IGraphNode<T>> resultNodes)
    {
        if (resultNodes == null)
            return;
        if (Array == null)
        {
            Log.Error("ArrayGraph array is null, check initialize or recycle");
            return;
        }
        resultNodes.Capacity = Array.Length;
        foreach (var graphNode in Array)
        {
            resultNodes.Add(graphNode);
        }
    }

    public override void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node)
    {
        if (node.Handle is FArrayGraphNodeHandle handle)
        {
            var right = CreateHandle(handle.Row, handle.Column + 1);
            var left = CreateHandle(handle.Row, handle.Column - 1);
            var up = CreateHandle(handle.Row +1, handle.Column);
            var down = CreateHandle(handle.Row -1, handle.Column);

            IGraphNode<T> tempNode = null;
            tempNode = FindNode(left);
            if(tempNode != null)
                resultNodes.Add(tempNode);
            tempNode = FindNode(right);
            if(tempNode != null)
                resultNodes.Add(tempNode);
            tempNode = FindNode(up);
            if(tempNode != null)
                resultNodes.Add(tempNode);
            tempNode = FindNode(down);
            if(tempNode != null)
                resultNodes.Add(tempNode);
        }

        return;

    }


    public override IGraphNode<T> FindNode(IGraphNodeHandle handle)
    {
        if (handle is FArrayGraphNodeHandle h)
        {
            if (h.Row < 0 || h.Row >= Row || h.Column < 0 || h.Column >= Column)
            {
                return null;
            }
            return Array[h.Row, h.Column];
        }

        return null;
    }
    

    public override void Clear()
    {
        OnClear();
        for (int i = 0; i < Array.Length; ++i)
        {
            var element = Array[i / Array.GetLength(0), i % Array.GetLength(0)];
            Array[i / Array.GetLength(0), i % Array.GetLength(0)] = null;
            if (element != null)
            {
                ReferencePool.Release(element);
            }
        }
    }

    public override void ForEach(Action<IGraphNode<T>> action)
    {
        if (action == null)
            return;
        
        for (int i = 0; i < Array.Length; ++i)
        {
            var element = Array[i / Array.GetLength(0), i % Array.GetLength(0)];
            if (element != null)
            {
                action.Invoke(element);
            }
        }
    }
    
    public override bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition)
    {
        bool result = false;
        nodeRelativePosition = float3.zero;

        if(false == WorldToGraph(worldPos, out IGraphNode<T> node))
        {
            //GameFrameworkLog.Info("");
            return result;
        }
        nodeRelativePosition = node.GetRelativePosition();
        return true;
    }
    
    public override bool WorldToGraph(float3 worldPos, out IGraphNode<T> node)
    {
        float3 relativeCoordinate = worldPos - WorldPosition;
        float3 originRelativeCoordinate = relativeCoordinate;
        node = null;
        if (relativeCoordinate.x < 0 || relativeCoordinate.z < 0)
        {
            return false;
        }

        //去掉间隙影响
        relativeCoordinate.x -= (int)relativeCoordinate.x * NodeMarginWidth;
        relativeCoordinate.z -= (int)relativeCoordinate.z * NodeMarginHeight;
        
        //去掉首格无margin的影响
        if (originRelativeCoordinate.x <= 1 && relativeCoordinate.x - (int)relativeCoordinate.x < NodeWidth/ 100.0f)
            relativeCoordinate.x = (int)relativeCoordinate.x - 1;
        if (originRelativeCoordinate.z <= 1 && relativeCoordinate.z - (int)relativeCoordinate.z < NodeHeight/100.0f)
            relativeCoordinate.z = (int)relativeCoordinate.z -1;
        
        int relativeRow = (int)math.floor(relativeCoordinate.z / NodeHeight);
        int relativeColumn = (int)math.floor(relativeCoordinate.x / NodeWidth);

        node = FindNode(CreateHandle(relativeRow, relativeColumn));
        return node != null;
    }
    
    public override bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos)
    {
        worldPos = float3.zero;
        if (handle is FArrayGraphNodeHandle h)
        {
            var node = FindNode(CreateHandle(h.Row, h.Column));
            if(node != null)
                worldPos = node.WorldPosition;
            else
            {
                return false;
            }
            return true;
        }

        return false;
    }

    public override float3 GetWorldPosition()
    {
        return WorldPosition;
    }

    protected virtual void OnClear()
    {
        
    }

    public FArrayGraphNodeHandle CreateHandle(int row, int column)
    {
        row = math.max(0, row);
        column = math.max(0, column);

        row = math.min(row, Row -1);
        column = math.min(column, Column -1);

        return new FArrayGraphNodeHandle(row, column);
    }
}


public struct FArrayGraphNodeHandle: IGraphNodeHandle
{
    public int Row { get; private set; }
    public int Column { get; private set; }

    public FArrayGraphNodeHandle(int row, int column)
    {
        this.Row = row;
        this.Column = column;
    }
    
    
}