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
        private IRng _obstacleRng;
        private IRng _collectableRng;

        [Inject]
        private void Initialize(IRng rng)
        {
            _rng = rng;
        }
        private void Awake()
        {
            var seed = _rng.InitialSeed;
            _obstacleRng = new SeededRng(seed ^ 0x51F15EED);
            _collectableRng = new SeededRng(seed ^ 0x51F15EED);

            GameplayRng.Obstacles = _obstacleRng;
            GameplayRng.Collectables = _collectableRng;
            GameplayRng.Instance = _obstacleRng;

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
            _obstaclePool = new ObstaclePool(stickPairPoolConfig, _obstacleRng);
            _collectableItemPool = new CollectableItemPool(collectableItemPoolConfig, _collectableRng);
        }
        private void InjectPools()
        {
            obstacleGenerator.InitializePools(_obstaclePool, _collectableItemPool);
        }
    }
}