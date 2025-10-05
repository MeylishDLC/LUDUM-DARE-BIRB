using System;
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
        private StickPairPool _stickPairPool;

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
        }
        private void InjectPools()
        {
            sticksGenerator.InitializePool(_stickPairPool);
        }
    }
}