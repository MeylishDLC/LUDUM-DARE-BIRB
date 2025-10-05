using System;
using System.Collections.Generic;
using Controller;
using EnvironmentObjects.Collectables;
using EnvironmentObjects.Obstacles;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace EnvironmentObjects.Sticks
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] private float startYOffset = 3f;
        [SerializeField] private ObstacleGeneratorConfig config;
        private ObstaclePool _obstaclePool;
        private CollectableItemPool _itemPool;
        private Transform _player;

        private readonly Queue<BaseObstacle> _activeObstacles = new();

        [Inject]
        public void Initialize(PlayerController playerController)
        {
            _player = playerController.transform;
        }

        public void InitializePools(ObstaclePool pool, CollectableItemPool itemPool)
        {
            _obstaclePool = pool;
            _itemPool = itemPool;
        }
        private void Start()
        {
            if (_obstaclePool == null || _itemPool == null)
            {
                Debug.LogError("Pools were not initialized");
                return;
            }

            SpawnInitialObstacles();
        }
        private void Update()
        {
            if (_activeObstacles.Count == 0)
            {
                return;
            }

            var top = GetTopObstacle();
            var bottom = _activeObstacles.Peek();
            
            if (_player.position.y + config.DistanceY * 3f > top.transform.position.y)
            {
                MoveLastToTop(top);
                return;
            }
            if (_player.position.y - config.DistanceY * 3f < bottom.transform.position.y)
            {
                MoveTopToLast(bottom);
            }
        }
        private void SpawnInitialObstacles()
        {
            var startY = _player.position.y - config.DistanceY * 2f + startYOffset;
            for (int i = 0; i < config.MaxPairs; i++)
            {
                var obstacle = GetRandomObstacle();

                if (obstacle is StickPair stickPair)
                {
                    InitializeStickPair(stickPair);
                }

                var pos = new Vector3(0f, startY + i * config.DistanceY, 0f);
                obstacle.transform.position = pos;
                _activeObstacles.Enqueue(obstacle);
            }
        }
        private void InitializeStickPair(StickPair stickPair)
        {
            var itemSetter = stickPair.GetComponentInChildren<CollectableItemSetter>();
            itemSetter.InitializePool(_itemPool);
        }
        private void MoveLastToTop(BaseObstacle top)
        {
            var recycled = _activeObstacles.Dequeue();
            //return to pool 
            recycled.gameObject.SetActive(false);

            //taking new random one
            var newObstacle = GetRandomObstacle();
            if (newObstacle is StickPair stickPair)
            {
                InitializeStickPair(stickPair);
            }

            var newPos = top.transform.position + Vector3.up * config.DistanceY;
            newObstacle.transform.position = newPos;

            _activeObstacles.Enqueue(newObstacle);
        }
        private void MoveTopToLast(BaseObstacle bottom)
        {
            var topList = new List<BaseObstacle>(_activeObstacles);
            var topPair = topList[^1];
            //return to pool
            topPair.gameObject.SetActive(false);

            //taking new random one
            var newObstacle = GetRandomObstacle();
            if (newObstacle is StickPair stickPair)
            {
                InitializeStickPair(stickPair);
            }

            var newPos = bottom.transform.position - Vector3.up * config.DistanceY;
            newObstacle.transform.position = newPos;
            
            topList.RemoveAt(topList.Count - 1);
            topList.Insert(0, newObstacle);

            _activeObstacles.Clear();
            foreach (var p in topList)
            {
                _activeObstacles.Enqueue(p);
            }
        }
        private BaseObstacle GetTopObstacle()
        {
            BaseObstacle top = null;
            foreach (var p in _activeObstacles)
            {
                top = p;
            }
            return top;
        }
        private BaseObstacle GetRandomObstacle()
        {
            if (_obstaclePool.TryGetFromPool(out var pair))
            {
                return pair;
            }
            throw new Exception("Stick pool is empty");
        }
    }
}