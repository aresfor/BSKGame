using System;
using System.Collections.Generic;

namespace Game.Client
{
    /// <summary>
    ///   <para>A stack based Pool.IObjectPool</para>
    /// </summary>
    public class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
    {
        private readonly List<T> _list;
        private readonly Func<T> _onCreate;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly int _maxSize;

        public int CountAll { get; private set; }
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _list.Count;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="createFunc"></param>
        /// <param name="actionOnGet">如果null，会执行IRecycle的OnGet</param>
        /// <param name="actionOnRelease">如果null，会执行IRecycle的OnRelease</param>
        /// <param name="actionOnDestroy">如果null，会执行IRecycle的OnDestroy</param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectPool(
            Func<T> createFunc,
            Action<T> actionOnGet = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null,
            int defaultCapacity = 10,
            int maxSize = 5000)
        {
            if (maxSize <= 0)
                throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
            _list = new List<T>(defaultCapacity);
            _onCreate = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _maxSize = maxSize;
            _onGet = actionOnGet;
            _onRelease = actionOnRelease;
            _onDestroy = actionOnDestroy;
        }

        public T Get()
        {
            T obj;
            if (_list.Count == 0)
            {
                obj = _onCreate();
                ++CountAll;
            }
            else
            {
                int index = _list.Count - 1;
                obj = _list[index];
                _list.RemoveAt(index);
            }

            Action<T> actionOnGet = _onGet;
            if (actionOnGet != null)
            {
                actionOnGet(obj);
            }
            else
            {
                if (obj is IRecycle recycleObj)
                {
                    recycleObj.OnGet();
                }
            }
                
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
            {
                actionOnRelease(element);
            }
            else
            {
                if (element is IRecycle recycleElement)
                {
                    recycleElement.OnRelease();
                }
            }
            
            if (CountInactive < _maxSize)
            {
                _list.Add(element);
            }
            else
            {
                // 超过最大size，销毁
                Action<T> actionOnDestroy = _onDestroy;
                if (actionOnDestroy != null)
                {
                    actionOnDestroy(element);
                }
                else
                {
                    if (element is IRecycle recycleElement)
                    {
                        recycleElement.OnDestroy();
                    }
                }
            }
        }
        
        public void Release(T element, bool collectionCheck)
        {
            if (collectionCheck && _list.Count > 0)
            {
                for (int index = 0; index < _list.Count; ++index)
                {
                    if (element == _list[index])
                        throw new InvalidOperationException(
                            "Trying to release an object that has already been released to the pool.");
                }
            }

            Release(element);
        }

        public void Clear()
        {
            if (_onDestroy != null)
            {
                foreach (T obj in _list)
                {
                    _onDestroy(obj);
                }
            }
            else
            {
                foreach (T obj in _list)
                {
                    if (obj is IRecycle recycleObj)
                    {
                        recycleObj.OnDestroy();
                    }
                }
            }
            _list.Clear();
            CountAll = 0;
        }

        public void Dispose() => Clear();
    }
}