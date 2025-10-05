using System;
using System.Collections.Generic;
using Controller;
using EnvironmentObjects.Collectables;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace EnvironmentObjects.Sticks
{
    public class SticksGenerator : MonoBehaviour
    {
        [SerializeField] private float startYOffset = 3f;
        [SerializeField] private SticksGeneratorConfig config;
        private StickPairPool _stickPairPool;
        private CollectableItemPool _itemPool;
        private Transform _player;

        private readonly Queue<StickPair> _activePairs = new();

        [Inject]
        public void Initialize(PlayerController playerController)
        {
            _player = playerController.transform;
        }

        public void InitializePools(StickPairPool pool, CollectableItemPool itemPool)
        {
            _stickPairPool = pool;
            _itemPool = itemPool;
        }
        private void Start()
        {
            if (_stickPairPool == null || _itemPool == null)
            {
                Debug.LogError("Pools were not initialized");
                return;
            }

            var startY = _player.position.y - config.DistanceY * 2f + startYOffset;
            for (int i = 0; i < config.MaxPairs; i++)
            {
                var pair = GetRandomPair();
                var itemSetter = pair.GetComponentInChildren<CollectableItemSetter>();
                itemSetter.InitializePool(_itemPool);

                var pos = new Vector3(0f, startY + i * config.DistanceY, 0f);
                pair.transform.position = pos;
                _activePairs.Enqueue(pair);
            }
        }
        private void Update()
        {
            if (_activePairs.Count == 0)
            {
                return;
            }

            var top = GetTopPair();
            var bottom = _activePairs.Peek();
            
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
        private void MoveLastToTop(StickPair top)
        {
            var recycled = _activePairs.Dequeue();
            var newPos = top.transform.position + Vector3.up * config.DistanceY;

            recycled.transform.position = newPos;
            recycled.gameObject.SetActive(true);

            _activePairs.Enqueue(recycled); 
        }
        private void MoveTopToLast(StickPair bottom)
        {
            var topList = new List<StickPair>(_activePairs);
            var topPair = topList[^1];
                
            var newPos = bottom.transform.position - Vector3.up * config.DistanceY;
            topPair.transform.position = newPos;

            topList.RemoveAt(topList.Count - 1);
            topList.Insert(0, topPair);

            _activePairs.Clear();
            foreach (var p in topList)
            {
                _activePairs.Enqueue(p);
            }
        }
        private StickPair GetTopPair()
        {
            StickPair top = null;
            foreach (var p in _activePairs)
            {
                top = p;
            }
            return top;
        }
        private StickPair GetRandomPair()
        {
            if (_stickPairPool.TryGetFromPool(out var pair))
            {
                return pair;
            }
            throw new Exception("Stick pool is empty");
        }
    }
}