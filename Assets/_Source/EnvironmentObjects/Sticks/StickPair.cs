using System;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    public class StickPair: MonoBehaviour, IPoolObject<StickPair>
    {
        [field: SerializeField] public Transform[] ItemSpawnPoints { get; private set; }
        public event Action<StickPair> OnObjectDisabled;
        private void OnDisable()
        {
            OnObjectDisabled?.Invoke(this);
            Debug.Log("Returned to pool");
        }
    }
}