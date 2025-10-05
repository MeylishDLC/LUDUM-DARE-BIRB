using System.Collections.Generic;
using System.Linq;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    public class StickPairPool: GenericPool<StickPair>
    {
        public StickPairPool(PoolConfig poolConfig) : base(poolConfig)
        { }
        public override bool TryGetFromPool(out StickPair instance)
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
        protected override StickPair InstantiateNewObject()
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