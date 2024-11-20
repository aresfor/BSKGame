using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 池化的数组
    /// 使用了System.Buffers.ArrayPool
    /// 出池的数组真实长度是2的n次方，封装了Length方法返回申请长度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct PooledArray<T> : IList<T>, IDisposable where T : new()
    {
        private static readonly ArrayPool<T> _arrayPool = ArrayPool<T>.Create(2000, 50);
        
        private T[] _items;
        private int _size;
        private bool _isInit;

        public int Length => _size;
        
        public static implicit operator T[](PooledArray<T> pooledArray)
        {
            return pooledArray.ToArray();
        }

        public static void Resize(ref PooledArray<T> array, int newSize)
        {
            if (newSize <= array._items.Length)
            {
                array._size = newSize;
                return;
            }
            var oldSize = array._size;
            array._size = newSize;
            var oldItems = array._items;
            var newItems = _arrayPool.Rent(newSize);
            Array.Copy(oldItems, newItems, oldSize);
            
            _arrayPool.Return(oldItems, true);
            array._items = newItems;
        }
        
        public PooledArray(int size)
        {
            _isInit = true;
            _size = size;
            _items = _arrayPool.Rent(size);
            
            if (_items[0] is not null)
                return;
            
            for (int i = 0; i < size; i++)
            {
                _items[i] = new T();
            }
        }
        
        public T[] ToArray()
        {
            T[] destArray = new T[_size];
            Array.Copy(_items, destArray, _size);
            return destArray;
        }
        
        public void ToArray(ref T[] result)
        {
            if (result == null)
                throw new NullReferenceException();
            if (result.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
            Array.Copy(_items, 0, result, 0, _size);
        }
        
        public bool InValid => !_isInit;
        
        #region IDispose

        public void Dispose()
        {
            if (_items != null)
            {
                _arrayPool.Return(_items, true);
                _items = null;
            }

            _size = -1;
            _isInit = false;
        }
        
        #endregion
        
        #region IList
        
        public IEnumerator<T> GetEnumerator()
        {
            return new NoAllocEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }

        public bool Contains(T item)
        {
            return Length != 0 && IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array != null && array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
            Array.Copy(_items, 0, array, arrayIndex, Length);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        public int Count => _size;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Collection is of a fixed size");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _items[index];
            }
            set
            {
                if (index < 0 || index > Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                _items[index] = value;
            }
        }
        
        #endregion
        
        struct NoAllocEnumerator : IEnumerator<T>
        {
            private readonly PooledArray<T> _array;
            private int _index;
            private T _current;
            private bool _exceeded;

            public NoAllocEnumerator(PooledArray<T> array) : this()
            {
                _array = array;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _array.Count)
                {
                    _current = _array[_index];
                    _index++;
                    return true;
                }
                else
                {
                    _index = _array.Count + 1;
                    _current = default(T);
                    _exceeded = true;
                    return false;
                }
            }

            public T Current => _current;

            Object IEnumerator.Current
            {
                get
                {
                    if (_exceeded)
                    {
                        throw new InvalidOperationException();
                    }

                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                _index = -1;
            }
        }
    }
}