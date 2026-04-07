using System.Linq;
using PoolSystem;
using Replay;
using UnityEngine;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItemPool: GenericPool<CollectableItem>
    {
        private readonly IRng _rng;

        public CollectableItemPool(PoolConfig poolConfig, IRng rng) : base(poolConfig)
        {
            _rng = rng;
            InitializeObjectPrefabs(poolConfig);  
            InitPool(ObjectPrefabs);
        }

        public override bool TryGetFromPool(out CollectableItem instance)
        {
            if (Pool.TryDequeue(out instance))
            {
                instance.gameObject.SetActive(true);
                return true;
            }

            if (AllObjects.Count < MaxPoolSize)
            {
                instance = InstantiateNewObject();
                instance.gameObject.SetActive(true);
                return true;
            }

            instance = null;
            return false;
        }

        public override void DisableAll()
        {
            foreach (var instance in AllObjects.Where(item => item.gameObject.activeSelf))
            {
                instance.gameObject.SetActive(false);
                ReturnToPool(instance);
            }
        }
        protected override CollectableItem InstantiateNewObject()
        {
            var randIndex = _rng.RangeInt(0, ObjectPrefabs.Length);
            var instance = Object.Instantiate(ObjectPrefabs[randIndex], ParentTransform);
            
            instance.gameObject.SetActive(false);
            instance.OnObjectDisabled += ReturnToPool;
            AllObjects.Add(instance);
            return instance;
        }
    }
}