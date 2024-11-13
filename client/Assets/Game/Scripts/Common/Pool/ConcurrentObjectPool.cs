using System;
using System.Collections.Concurrent;

namespace Game.Client
{
    /// <summary>
    /// 对象池, 线程安全, 无重复回池检查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentObjectPool<T> : IDisposable, IUnsafeObjectPool<T> where T : class
    {
        private readonly ConcurrentQueue<T> _concurrentQueue;
        private readonly Func<T> _onCreate;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly int _maxSize;

        public ConcurrentObjectPool(
            Func<T> createFunc,
            Action<T> actionOnGet = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null)
        {
            _concurrentQueue = new ConcurrentQueue<T>();
            _onCreate = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _onGet = actionOnGet;
            _onRelease = actionOnRelease;
            _onDestroy = actionOnDestroy;
        }
        
        public T Get()
        {
            if (!_concurrentQueue.TryDequeue(out var obj))
            {
                obj = _onCreate();
            }
            
            Action<T> actionOnGet = _onGet;
            if (actionOnGet != null)
                actionOnGet(obj);
            return obj;
        }
        
        public PooledObject<T> Get(out T v)
        {
            return new PooledObject<T>(v = Get(), this);
        }
        
        public void Release(T element)
        {
            Action<T> actionOnRelease = _onRelease;
            if (actionOnRelease != null)
                actionOnRelease(element);
            _concurrentQueue.Enqueue(element);
        }
        
        public void Clear()
        {
            if (_onDestroy != null)
            {
                while (_concurrentQueue.TryDequeue(out var obj))
                {
                    _onDestroy?.Invoke(obj);
                }
            }
        }

        public void Dispose() => Clear();
    }
}