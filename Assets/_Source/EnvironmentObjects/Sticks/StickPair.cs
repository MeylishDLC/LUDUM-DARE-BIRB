using System;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    public class StickPair: MonoBehaviour, IPoolObject<StickPair>
    {
        public event Action<StickPair> OnObjectDisabled;
        public event Action<StickPair> OnObjectEnabled;
        
        [field: SerializeField] public Transform[] ItemSpawnPoints { get; private set; }
        private void OnEnable()
        {
            OnObjectEnabled?.Invoke(this);
        }
        private void OnDisable()
        {
            OnObjectDisabled?.Invoke(this);
        }
    }
}