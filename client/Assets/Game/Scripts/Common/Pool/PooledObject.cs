using System;

namespace Game.Client
{
    public readonly struct PooledObject<T> : IDisposable where T : class
    {
        private readonly T _toReturn;
        private readonly IUnsafeObjectPool<T> _pool;

        internal PooledObject(T value, IUnsafeObjectPool<T> pool)
        {
            _toReturn = value;
            _pool = pool;
        }

        void IDisposable.Dispose()
        {
            if (_pool is IObjectPool<T> safePool)
            {
                safePool.Release(_toReturn, true);
                return;
            }
            _pool.Release(_toReturn);
        }
    }
}