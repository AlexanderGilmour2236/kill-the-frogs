using UnityEngine;
using UnityEngine.Pool;

namespace Utils
{
    public abstract class Pool<T> where T: MonoBehaviour
    {
        protected readonly T _prefab;
        protected readonly int _maxPoolSize;
        protected readonly int _defaultCapacity;
        protected readonly bool _collectionChecks;

        public Pool(T prefab, int maxPoolSize, int defaultCapacity, bool collectionChecks = true)
        {
            _prefab = prefab;
            _maxPoolSize = maxPoolSize;
            _defaultCapacity = defaultCapacity;
            _collectionChecks = collectionChecks;
        }

        private IObjectPool<T> _pool;

        public IObjectPool<T> pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<T>(createPooledItem, onTakeFromPool, onReturnedToPool,
                        onDestroyPoolObject, _collectionChecks, _defaultCapacity, _maxPoolSize);
                }

                return _pool;
            }
        }

        protected virtual T createPooledItem()
        {
            return Object.Instantiate(_prefab);
        }

        protected abstract void onTakeFromPool(T pooled);

        protected abstract void onReturnedToPool(T pooled);

        protected virtual void onDestroyPoolObject(T pooled)
        {
            Object.Destroy(pooled.gameObject);
        }
    }
}