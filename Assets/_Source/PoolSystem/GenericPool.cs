using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PoolSystem
{
    public abstract class GenericPool<T>: IPool<T> where T : Object, IPoolObject<T>
    {
        protected readonly List<T> AllObjects = new();
        protected readonly Queue<T> Pool = new();
        protected readonly int MaxPoolSize;
        protected readonly Transform ParentTransform;
        protected T[] ObjectPrefabs;

        private readonly int _startPoolSize;
        
        protected GenericPool(PoolConfig poolConfig)
        {
            var parent = new GameObject("Pool");
            ParentTransform = parent.transform;

            MaxPoolSize = poolConfig.MaxPoolSize; 
            _startPoolSize = poolConfig.InitialPoolSize;
            InitializeObjectPrefabs(poolConfig);  

            InitPool(ObjectPrefabs);
        }
        public void InitPool(T[] prefabs)
        {
            for (int i = 0; i < _startPoolSize; i++)
            {
                var instance = InstantiateNewObject();
                Pool.Enqueue(instance);
            }
        }
        public abstract bool TryGetFromPool(out T instance);
        public void ReturnToPool(T instance)
        {
            Pool.Enqueue(instance);
        }
        public abstract void DisableAll();
        public void CleanUp()
        {
            foreach (var instance in AllObjects)
            {
                instance.OnObjectDisabled -= ReturnToPool;
            }
        }
        protected abstract T InstantiateNewObject();

        private void InitializeObjectPrefabs(PoolConfig poolConfig)
        {
            ObjectPrefabs = new T[poolConfig.Prefabs.Length];

            for (int i = 0; i < ObjectPrefabs.Length; i++)
            {
                var tPrefab = poolConfig.Prefabs[i].GetComponent<T>();
                ObjectPrefabs[i] = tPrefab;
            }
        }
       
    }
}