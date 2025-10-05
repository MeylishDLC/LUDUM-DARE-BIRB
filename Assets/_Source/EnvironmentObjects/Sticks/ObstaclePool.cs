using System.Collections.Generic;
using System.Linq;
using EnvironmentObjects.Obstacles;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    public class ObstaclePool: GenericPool<BaseObstacle>
    {
        public ObstaclePool(PoolConfig poolConfig) : base(poolConfig)
        { }
        public override bool TryGetFromPool(out BaseObstacle instance)
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
            foreach (var instance in AllObjects.Where(pair => pair.gameObject.activeSelf))
            {
                instance.gameObject.SetActive(false);
                ReturnToPool(instance);
            }
        }
        protected override BaseObstacle InstantiateNewObject()
        {
            var randIndex = Random.Range(0, ObjectPrefabs.Length);
            var instance = Object.Instantiate(ObjectPrefabs[randIndex], ParentTransform);
            
            instance.gameObject.SetActive(false);
            instance.OnObjectDisabled += ReturnToPool;
            AllObjects.Add(instance);
            return instance;
        }
    }
}