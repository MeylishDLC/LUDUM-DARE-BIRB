using System;
using EnvironmentObjects.Collectables;
using EnvironmentObjects.Sticks;
using PoolSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class Bootstrapper: MonoBehaviour
    {
        [FormerlySerializedAs("sticksGenerator")] [SerializeField] private ObstacleGenerator obstacleGenerator;
        
        [Header("Pools")]
        [SerializeField] private PoolConfig stickPairPoolConfig;
        [SerializeField] private PoolConfig collectableItemPoolConfig;
        
        private ObstaclePool _obstaclePool;
        private CollectableItemPool _collectableItemPool;

        private void Awake()
        {
            CreatePools();
            InjectPools();
        }
        private void OnDestroy()
        {
            _obstaclePool.CleanUp();
            _collectableItemPool.CleanUp();
        }
        private void CreatePools()
        {
            _obstaclePool = new ObstaclePool(stickPairPoolConfig);
            _collectableItemPool = new CollectableItemPool(collectableItemPoolConfig);
        }
        private void InjectPools()
        {
            obstacleGenerator.InitializePools(_obstaclePool, _collectableItemPool);
        }
    }
}