using EnvironmentObjects.Collectables;
using EnvironmentObjects.Sticks;
using PoolSystem;
using Replay;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class Bootstrapper: MonoBehaviour
    {
        [SerializeField] private ObstacleGenerator obstacleGenerator;
        
        [Header("Pools")]
        [SerializeField] private PoolConfig stickPairPoolConfig;
        [SerializeField] private PoolConfig collectableItemPoolConfig;
        
        private ObstaclePool _obstaclePool;
        private CollectableItemPool _collectableItemPool;
        private IRng _rng;

        [Inject]
        private void Initialize(IRng rng)
        {
            _rng = rng;
        }
        private void Awake()
        {
            GameplayRng.Instance = _rng;
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
            _obstaclePool = new ObstaclePool(stickPairPoolConfig, _rng);
            _collectableItemPool = new CollectableItemPool(collectableItemPoolConfig, _rng);
        }
        private void InjectPools()
        {
            obstacleGenerator.InitializePools(_obstaclePool, _collectableItemPool);
        }
    }
}