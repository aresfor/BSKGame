namespace Game.Core;

using System;
using System.Collections.Generic;

/// <summary>
/// 最大堆优先队列实现
/// </summary>
/// <typeparam name="T"></typeparam>
public class PriorityQueue<T>
{
    private List<(T Item, float Priority)> heap = new List<(T, float)>();

    public int Count => heap.Count;

    /// <summary>
    /// 入队：将元素加入优先队列
    /// </summary>
    public void Enqueue(T item, float priority)
    {
        heap.Add((item, priority));
        HeapifyUp(heap.Count - 1);
    }

    /// <summary>
    /// 出队：取出优先级最低的元素
    /// </summary>
    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        T item = heap[0].Item; // 取出堆顶元素
        heap[0] = heap[heap.Count - 1]; // 将最后一个元素放到堆顶
        heap.RemoveAt(heap.Count - 1); // 删除最后一个元素
        HeapifyDown(0); // 调整堆

        return item;
    }

    /// <summary>
    /// 查看堆顶元素，但不移除
    /// </summary>
    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");
        return heap[0].Item;
    }

    /// <summary>
    /// 判断队列中是否包含某个元素
    /// </summary>
    public bool Contains(T item)
    {
        return heap.Exists(e => EqualityComparer<T>.Default.Equals(e.Item, item));
    }

    /// <summary>
    /// 上浮调整
    /// </summary>
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2; // 父节点索引
            if (heap[index].Priority >= heap[parentIndex].Priority)
                break;

            Swap(index, parentIndex);
            index = parentIndex; // 更新当前索引为父节点索引
        }
    }

    /// <summary>
    /// 下沉调整
    /// </summary>
    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;

        while (true)
        {
            int leftChild = 2 * index + 1; // 左子节点索引
            int rightChild = 2 * index + 2; // 右子节点索引
            int smallest = index;

            if (leftChild <= lastIndex && heap[leftChild].Priority < heap[smallest].Priority)
                smallest = leftChild;

            if (rightChild <= lastIndex && heap[rightChild].Priority < heap[smallest].Priority)
                smallest = rightChild;

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    /// <summary>
    /// 交换堆中的两个元素
    /// </summary>
    private void Swap(int i, int j)
    {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    /// <summary>
    /// 清理堆
    /// </summary>
    public void Clear()
    {
        heap.Clear();
    }
}
