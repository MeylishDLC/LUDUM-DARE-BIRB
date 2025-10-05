using System;
using EnvironmentObjects.Sticks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItemSetter: MonoBehaviour
    {
        [Range(0,100)] [SerializeField] private int itemPlaceChance = 50;
        [SerializeField] private StickPair stickPair;

        private CollectableItemPool _itemPool;
        private CollectableItem _currentItem;
        private Transform _origParent;
        public void InitializePool(CollectableItemPool pool)
        {
            Debug.Log("CollectableItemPool initialized");
            _itemPool = pool;
            stickPair.OnObjectEnabled += PlaceRandomItem;
            stickPair.OnObjectDisabled += RemoveRandomItem;
            
            PlaceRandomItem(null);
        }
        private void PlaceRandomItem(StickPair _)
        {
            if (!CheckPlaceChance())
            {
                return;
            }
            
            if (_itemPool.TryGetFromPool(out var item))
            {
                _currentItem = item;
                _origParent = _currentItem.gameObject.transform.parent;
                var randomPoint = GetRandomPoint();
                
                _currentItem.gameObject.transform.SetParent(randomPoint);
                _currentItem.gameObject.transform.position = randomPoint.position;
                _currentItem.OnCollected += StopPlacingItems;
            }
        }
        private void RemoveRandomItem(StickPair _)
        {
            if (!_currentItem)
            {
                return;
            }
            _currentItem.gameObject.SetActive(false);
            _currentItem.OnCollected -= StopPlacingItems;
            _origParent = null;
            _currentItem = null;
        }

        private bool CheckPlaceChance()
        {
            var randomChance = Random.Range(0, 100);
            
            if (randomChance <= itemPlaceChance)
            {
                return true;
            }
            return false;
        }

        private Transform GetRandomPoint()
        {
            var point = stickPair.ItemSpawnPoints[Random.Range(0, stickPair.ItemSpawnPoints.Length)];
            return point;
        }
        private void StopPlacingItems()
        {
            if (_currentItem)
            {
                _currentItem.OnCollected -= StopPlacingItems;
            }
            Destroy(gameObject);
        }
    }
}