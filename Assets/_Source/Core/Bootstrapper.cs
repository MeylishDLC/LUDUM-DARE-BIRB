using System;
using EnvironmentObjects.Collectables;
using EnvironmentObjects.Sticks;
using PoolSystem;
using UnityEngine;

namespace Core
{
    public class Bootstrapper: MonoBehaviour
    {
        [SerializeField] private SticksGenerator sticksGenerator;
        
        [Header("Pools")]
        [SerializeField] private PoolConfig stickPairPoolConfig;
        [SerializeField] private PoolConfig collectableItemPoolConfig;
        
        private StickPairPool _stickPairPool;
        private CollectableItemPool _collectableItemPool;

        private void Awake()
        {
            CreatePools();
            InjectPools();
        }
        private void OnDestroy()
        {
            _stickPairPool.CleanUp();
        }
        private void CreatePools()
        {
            _stickPairPool = new StickPairPool(stickPairPoolConfig);
            _collectableItemPool = new CollectableItemPool(collectableItemPoolConfig);
        }
        private void InjectPools()
        {
            sticksGenerator.InitializePools(_stickPairPool, _collectableItemPool);
        }
    }
}